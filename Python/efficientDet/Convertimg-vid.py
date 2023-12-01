import cv2
import os

def images_to_video(image_folder, output_video_path, fps=30):
    images = [img for img in os.listdir(image_folder) if img.endswith(".png") or img.endswith(".jpg")]
    frame = cv2.imread(os.path.join(image_folder, images[0]))

    height, width, layers = frame.shape

    # Change VideoWriter_fourcc to 'MJPG' for AVI format
    fourcc = cv2.VideoWriter_fourcc(*'mp4v')
    video = cv2.VideoWriter(output_video_path, fourcc, fps, (width, height))

    for image in images:
        video.write(cv2.imread(os.path.join(image_folder, image)))

    cv2.destroyAllWindows()
    video.release()

if __name__ == "__main__":
    input_folder = r"C:\Users\jonas\Documents\GitHub\efficientDet\data\COCO\images\val2017"
    output_video = "output.mp4"
    frames_per_second = 30

    images_to_video(input_folder, output_video, frames_per_second)
