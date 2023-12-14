import os
import argparse
import torch
import torch.nn as nn
from torch.utils.data import DataLoader
from torchvision import transforms
from src.dataset import CocoDataset, Resizer, Normalizer, Augmenter, collater
from src.model import EfficientDet
from tensorboardX import SummaryWriter
import shutil
import numpy as np
from tqdm.autonotebook import tqdm
#RUN WITH THIS PLS DONT DO IT WITOUT I WILL CRY python train.py --resume_checkpoint C:\Users\jonas\Documents\GitHub\MED7-TheFishening\Python\efficientDet\trained_models\checkpoint.pth
def get_args():
    parser = argparse.ArgumentParser(
        "EfficientDet: Scalable and Efficient Object Detection implementation by Signatrix GmbH")
    parser.add_argument("--image_size", type=int, default=512, help="The common width and height for all images")
    parser.add_argument("--batch_size", type=int, default=4, help="The number of images per batch")
    parser.add_argument("--lr", type=float, default=1e-4)
    parser.add_argument('--alpha', type=float, default=0.25)
    parser.add_argument('--gamma', type=float, default=1.5)
    parser.add_argument("--num_epochs", type=int, default=50)
    parser.add_argument("--test_interval", type=int, default=1, help="Number of epochs between testing phases")
    parser.add_argument("--es_min_delta", type=float, default=0.0,
                        help="Early stopping's parameter: minimum change loss to qualify as an improvement")
    parser.add_argument("--es_patience", type=int, default=0,
                        help="Early stopping's parameter: number of epochs with no improvement after which training will be stopped. Set to 0 to disable this technique.")
    parser.add_argument("--data_path", type=str, default="data/COCO", help="the root folder of the dataset")
    parser.add_argument("--log_path", type=str, default="tensorboard/signatrix_efficientdet_coco")
    parser.add_argument("--saved_path", type=str, default="trained_models")
    parser.add_argument("--resume_checkpoint", type=str, default=None, help="Path to a checkpoint file to resume training from")
    args = parser.parse_args()
    return args

def load_checkpoint(model, optimizer, scheduler, checkpoint_path):
    checkpoint = torch.load(checkpoint_path)
    model.load_state_dict(checkpoint['model_state_dict'])
    optimizer.load_state_dict(checkpoint['optimizer_state_dict'])
    scheduler.load_state_dict(checkpoint['scheduler_state_dict'])
    epoch = checkpoint['epoch']
    loss = checkpoint['loss']
    return model, optimizer, scheduler, epoch, loss

def save_checkpoint(model, optimizer, scheduler, epoch, loss, checkpoint_path):
    print("Saved")
    torch.save({
        'epoch': epoch,
        'model_state_dict': model.state_dict(),
        'optimizer_state_dict': optimizer.state_dict(),
        'scheduler_state_dict': scheduler.state_dict(),
        'loss': loss,
        
    }, checkpoint_path)

def train(opt):
    num_gpus = 1
    if torch.cuda.is_available():
        num_gpus = torch.cuda.device_count()
        torch.cuda.manual_seed(123)
    else:
        torch.manual_seed(123)

    training_params = {"batch_size": opt.batch_size * num_gpus,
                       "shuffle": True,
                       "drop_last": True,
                       "collate_fn": collater,
                       "num_workers": 16}

    test_params = {"batch_size": opt.batch_size,
                   "shuffle": False,
                   "drop_last": False,
                   "collate_fn": collater,
                   "num_workers": 16}

    training_set = CocoDataset(root_dir=opt.data_path, set="train2017",
                               transform=transforms.Compose([Normalizer(), Augmenter(), Resizer()]))
    training_generator = DataLoader(training_set, **training_params)

    test_set = CocoDataset(root_dir=opt.data_path, set="val2017",
                           transform=transforms.Compose([Normalizer(), Resizer()]))
    test_generator = DataLoader(test_set, **test_params)

    model = EfficientDet(num_classes=training_set.num_classes())
    

    if os.path.isdir(opt.log_path):
        shutil.rmtree(opt.log_path)
    os.makedirs(opt.log_path)

    if not os.path.isdir(opt.saved_path):
        os.makedirs(opt.saved_path)

    writer = SummaryWriter(opt.log_path)
    if torch.cuda.is_available():
        model = model.cuda()
        model = nn.DataParallel(model)

    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
    model = model.to(device)

    optimizer = torch.optim.Adam(model.parameters(), opt.lr)
    scheduler = torch.optim.lr_scheduler.ReduceLROnPlateau(optimizer, patience=3, verbose=True)

    start_epoch = 0
    best_loss = 1e5
    best_epoch = 0

    if opt.resume_checkpoint:
        model, optimizer, scheduler, start_epoch, best_loss = load_checkpoint(
            model, optimizer, scheduler, opt.resume_checkpoint)
        start_epoch+=1
        print(start_epoch)
        print("loaded checkpoint")


    model.train()

    num_iter_per_epoch = len(training_generator)
    for epoch in range(start_epoch, opt.num_epochs):
        model.train()
        if torch.cuda.is_available():
            model.module.freeze_bn()
        else:
            model.freeze_bn()
        epoch_loss = []
        progress_bar = tqdm(training_generator)
        for iter, data in enumerate(progress_bar):
            try:
                optimizer.zero_grad()
                
                cls_loss, reg_loss = model([data['img'].float().to(device), data['annot'].to(device)])

                cls_loss = cls_loss.mean()
                reg_loss = reg_loss.mean()
                loss = cls_loss + reg_loss
                if loss == 0:
                    continue
                loss.backward()
                torch.nn.utils.clip_grad_norm_(model.parameters(), 0.1)
                optimizer.step()
                epoch_loss.append(float(loss))
                total_loss = np.mean(epoch_loss)

                progress_bar.set_description(
                    'Epoch: {}/{}. Iteration: {}/{}. Cls loss: {:.5f}. Reg loss: {:.5f}. Batch loss: {:.5f} Total loss: {:.5f}'.format(
                        epoch + 1, opt.num_epochs, iter + 1, num_iter_per_epoch, cls_loss, reg_loss, loss,
                        total_loss))
                writer.add_scalar('Train/Total_loss', total_loss, epoch * num_iter_per_epoch + iter)
                writer.add_scalar('Train/Regression_loss', reg_loss, epoch * num_iter_per_epoch + iter)
                writer.add_scalar('Train/Classfication_loss (focal loss)', cls_loss, epoch * num_iter_per_epoch + iter)
        
            except Exception as e:
                print(e)
                continue
        
        scheduler.step(np.mean(epoch_loss))
        torch.cuda.empty_cache()

        if epoch % opt.test_interval == 0:
            model.eval()
            loss_regression_ls = []
            loss_classification_ls = []
            for iter, data in enumerate(test_generator):
                with torch.no_grad():
                    if torch.cuda.is_available():
                        cls_loss, reg_loss = model([data['img'].to(device).float(), data['annot'].to(device)])
                    else:
                        cls_loss, reg_loss = model([data['img'].float(), data['annot']])

                    cls_loss = cls_loss.mean()
                    reg_loss = reg_loss.mean()

                    loss_classification_ls.append(float(cls_loss))
                    loss_regression_ls.append(float(reg_loss))

            cls_loss = np.mean(loss_classification_ls)
            reg_loss = np.mean(loss_regression_ls)
            loss = cls_loss + reg_loss

            print(
                'Epoch: {}/{}. Classification loss: {:1.5f}. Regression loss: {:1.5f}. Total loss: {:1.5f}'.format(
                    epoch + 1, opt.num_epochs, cls_loss, reg_loss,
                    np.mean(loss)))
            writer.add_scalar('Test/Total_loss', loss, epoch)
            writer.add_scalar('Test/Regression_loss', reg_loss, epoch)
            writer.add_scalar('Test/Classfication_loss (focal loss)', cls_loss, epoch)

            if loss + opt.es_min_delta < best_loss:
                best_loss = loss
                best_epoch = epoch
                torch.save(model, os.path.join(opt.saved_path, "signatrix_efficientdet_coco.pth"))

                dummy_input = torch.rand(opt.batch_size, 3, 512, 512)
                if torch.cuda.is_available():
                    dummy_input = dummy_input.to(device)
                if isinstance(model, nn.DataParallel):
                    model.module.backbone_net.model.set_swish(memory_efficient=False)

                    torch.onnx.export(model.module, dummy_input,
                                      os.path.join(opt.saved_path, "signatrix_efficientdet_coco.onnx"),
                                      verbose=False)
                    model.module.backbone_net.model.set_swish(memory_efficient=True)
                else:
                    model.backbone_net.model.set_swish(memory_efficient=False)

                    torch.onnx.export(model, dummy_input,
                                      os.path.join(opt.saved_path, "signatrix_efficientdet_coco.onnx"),
                                      verbose=False)
                    model.backbone_net.model.set_swish(memory_efficient=True)

            # Save checkpoint after each epoch
            save_checkpoint(model, optimizer, scheduler, epoch, loss,
                            os.path.join(opt.saved_path, "checkpoint.pth"))
            

            # Early stopping
            if epoch - best_epoch > opt.es_patience > 0:
                print("Stop training at epoch {}. The lowest loss achieved is {}".format(epoch, loss))
                break

    writer.close()

if __name__ == "__main__":
    opt = get_args()
    train(opt)