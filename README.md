# MED7-TheFishening
7th Semeter medialogy project

# Data collection

Open the unity project, best to use the same unity version.

Start the scene, select the camera, enable the perception component, and data will now be saved in SOLO format.

# EffecientDet How to use:

Data MUST HAVE the following structure:

data->COCO->annotations

annotation names = instances_train2017 and instances_val2017 --Note, these does not need a folder each as the image folders do.

data->COCO->images 

image folder names = train2017 and val2017 --Note, these should both be folders

Install all packages used by different scripts, If you have an Nvidia GPU getting Nvidia CUDA helps with speed of training.

Replace relevant paths in different scripts to match your paths.

To train with GPU, go to the utils.py file line 63 for relevant instructions.

