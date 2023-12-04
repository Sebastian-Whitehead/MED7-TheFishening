import json
import datetime
import random
import shutil
import os
from json import write_to_file

def split_data(data, split_ratio):
    # Get the total number of images
    total_images = len(data['images'])

    # Calculate the number of images for the first split
    first_split_size = int(total_images * split_ratio)

    # Randomly select images for the first split
    first_split_images = random.sample(data['images'], first_split_size)

    # Get the ids of the selected images
    first_split_image_ids = {image['id'] for image in first_split_images}

    # Select annotations for the first split
    first_split_annotations = [annotation for annotation in data['annotations'] if annotation['image_id'] in first_split_image_ids]

    # Create the first split
    first_split = {
        'info': data['info'],
        'licenses': data['licenses'],
        'categories': data['categories'],
        'images': first_split_images,
        'annotations': first_split_annotations
    }

    # Select images for the second split
    second_split_images = [image for image in data['images'] if image['id'] not in first_split_image_ids]

    # Select annotations for the second split
    second_split_annotations = [annotation for annotation in data['annotations'] if annotation['image_id'] not in first_split_image_ids]

    # Create the second split
    second_split = {
        'info': data['info'],
        'licenses': data['licenses'],
        'categories': data['categories'],
        'images': second_split_images,
        'annotations': second_split_annotations
    }

    return first_split, second_split

def copy_images(data, folder_name):
    # Create the folder if it doesn't exist
    if not os.path.exists(folder_name):
        os.makedirs(folder_name)

    # Copy the images to the new folder
    for image in data['images']:
        shutil.copy(os.path.join('images', image['file_name']), folder_name)

# Load data from the merged file
with open('merged_file.json', 'r') as f:
    data = json.load(f)

# Split the data
split_ratio = 0.8  # 80/20 split
first_split, second_split = split_data(data, split_ratio)

# Write the splits to new files
write_to_file(first_split, 'first_split.json')
write_to_file(second_split, 'second_split.json')

# Copy the images to new folders
copy_images(first_split, 'first_split_images')
copy_images(second_split, 'second_split_images')

print("DONE!")
