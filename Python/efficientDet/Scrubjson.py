import os
import json
#This file removes references from the JSON file to non existing images.
def filter_json(json_file_path, images_directory):
    # Load JSON data
    with open(json_file_path, 'r') as json_file:
        data = json.load(json_file)

    # Check if 'images' key exists in data
    if 'images' in data:
        # Filter out rows without a corresponding image file
        filtered_data = [row for row in data['images'] if image_exists(images_directory, row['file_name'])]

        # Update the 'images' key in the original data
        data['images'] = filtered_data

        # Save the filtered data back to the JSON file
        with open(json_file_path, 'w') as json_file:
            json.dump(data, json_file, indent=2)
    else:
        print("'images' key not found in JSON data.")

def image_exists(images_directory, image_name):
    # Check if the image file exists
    image_path = os.path.join(images_directory, image_name)
    return os.path.exists(image_path)

if __name__ == "__main__":
    # Replace 'your_data.json' with the path to your JSON file
    json_file_path = r"your_data.json"

    # Replace 'your_images_directory' with the path to the directory containing your images
    images_directory = r'your_images_directory'

    # Call the filter_json function
    filter_json(json_file_path, images_directory)
