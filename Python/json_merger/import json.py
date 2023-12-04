import json
import datetime

# Define your info variables here
info = {
    "Description": "Merged dataset",
    "url": "Not Set",
    "version": "1.0.0",
    "year": 2023,
    "Contributor": "Your Name",
    "date_created": datetime.datetime.now().strftime("%d-%m-%Y")  # Set date_created to current date
}

def merge_files(data1, data2):
    # Use the predefined info
    merged_data = {'info': info}

    # Merge licenses
    merged_data['licenses'] = list(set(data1['licenses'] + data2['licenses']))

    # Merge categories, merge duplicates
    categories = data1['categories'] + data2['categories']
    names = [category['name'].lower() for category in categories]
    merged_categories = []
    for name in set(names):
        # Merge categories with the same name
        same_name_categories = [category for category in categories if category['name'].lower() == name]
        merged_category = same_name_categories[0]
        for category in same_name_categories[1:]:
            category_copy = category.copy()
            category_copy.pop('name', None)  # Remove 'name' from the copy
            category_copy.pop('id', None)  # Remove 'id' from the copy
            merged_category.update(category_copy)  # Update the category with the values from the other categories with the same name
        merged_category['name'] = merged_category['name'].lower()  # Convert the name to lowercase
        merged_category.pop('keypoints', None)  # Remove 'keypoints'
        merged_category.pop('skeleton', None)  # Remove 'skeleton'
        merged_categories.append(merged_category)
    merged_data['categories'] = merged_categories



    # Merge images
    for image in data2['images']:
        image['id'] += 100000  # Add 100000 to all id's in the second file
    merged_data['images'] = data1['images'] + data2['images']

    # Merge annotations
    for annotation in data2['annotations']:
        annotation['id'] += 100000  # Add 100000 to all id's in the second file
        annotation['image_id'] += 100000  # Add 100000 to all image_id's in the second file
    merged_data['annotations'] = data1['annotations'] + data2['annotations']

    return merged_data

def write_to_file(data, file_name):
    with open(file_name, 'w') as f:
        json.dump(data, f, indent=4)

# Load data from the first file
with open('bbox.json', 'r') as f:
    data1 = json.load(f)

# Load data from the second file
with open('instances_val2017.json', 'r') as f:
    data2 = json.load(f)

# Merge the data
merged_data = merge_files(data1, data2)

# Write the merged data to a new file
write_to_file(merged_data, 'merged_file.json')

print("DONE!")
