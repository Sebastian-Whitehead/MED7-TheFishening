import json
import os
import random  # Import the random module
from shutil import copyfile
from pycocotools.coco import COCO

def split_coco_dataset(annotations_path, images_path, output_path, train_percentage=0.8):
    # Create output directories
    train_dir = os.path.join(output_path, 'train2017')
    val_dir = os.path.join(output_path, 'val2017')

    os.makedirs(train_dir, exist_ok=True)
    os.makedirs(val_dir, exist_ok=True)

    # Load COCO annotations
    coco = COCO(annotations_path)

    # Get all image IDs and shuffle them randomly
    image_ids = coco.getImgIds()
    random.shuffle(image_ids)

    # Split image IDs into training and validation sets
    num_images = len(image_ids)
    num_train = int(num_images * train_percentage)
    train_image_ids = image_ids[:num_train]
    val_image_ids = image_ids[num_train:]

    # Split annotations into training and validation dictionaries
    train_annotations = coco.loadAnns(coco.getAnnIds(imgIds=train_image_ids))
    val_annotations = coco.loadAnns(coco.getAnnIds(imgIds=val_image_ids))

    # Copy images to the corresponding directories
    for image_id in train_image_ids:
        image_info = coco.loadImgs(image_id)[0]
        image_path = os.path.join(images_path, image_info['file_name'])
        copyfile(image_path, os.path.join(train_dir, image_info['file_name']))

    for image_id in val_image_ids:
        image_info = coco.loadImgs(image_id)[0]
        image_path = os.path.join(images_path, image_info['file_name'])
        copyfile(image_path, os.path.join(val_dir, image_info['file_name']))

    # Create training and validation datasets
    train_dataset = {
        'info': coco.dataset.get('info', {}),
        'licenses': coco.dataset.get('licenses', []),
        'categories': coco.dataset['categories'],
        'images': coco.loadImgs(train_image_ids),
        'annotations': train_annotations,
    }

    val_dataset = {
        'info': coco.dataset.get('info', {}),
        'licenses': coco.dataset.get('licenses', []),
        'categories': coco.dataset['categories'],
        'images': coco.loadImgs(val_image_ids),
        'annotations': val_annotations,
    }

    # Save training and validation datasets to JSON files
    with open(os.path.join(output_path, 'train2017.json'), 'w') as train_json_file:
        json.dump(train_dataset, train_json_file)

    with open(os.path.join(output_path, 'val2017.json'), 'w') as val_json_file:
        json.dump(val_dataset, val_json_file)

if __name__ == "__main__":
    annotations_path = r'C:\Users\jonas\Desktop\Data\coco\bbox.json'
    images_path = r'C:\Users\jonas\Desktop\Data\coco\images'
    output_path = r'C:\Users\jonas\Desktop\Data'

    split_coco_dataset(annotations_path, images_path, output_path)
