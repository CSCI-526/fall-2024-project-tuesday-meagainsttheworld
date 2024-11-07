import cv2
import numpy as np
from scipy.ndimage import gaussian_filter
import csv

# Fetch data from the CSV file
def fetch_data_from_csv(file_path):
    data_points = []
    with open(file_path, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        for row in csvreader:
            level_number = int(row['currLevel'][5])
            x = float(row['playerX'])
            y = float(row['playerY'])
            data_points.append((level_number, x, y))
    return data_points

# Transform coordinates from [-32, 32] to the target range [0, 2560] for x and [0, 1440] for y
def transform_coordinates(x, y, x_range=-32, y_range=-18, x_target=2560, y_target=1440):
    x_transformed = (x - x_range) * x_target / 64
    y_transformed = (y - y_range) * y_target / (-36) + y_target
    return x_transformed, y_transformed

# Apply heatmap to images based on transformed coordinates
def apply_heatmap_to_images(images, data_points, heatmap_intensity=1.0, blur_radius=40):
    heatmaps = {i: np.zeros((image.shape[0], image.shape[1]), dtype=np.float32) for i, image in enumerate(images)}

    for image_number, x, y in data_points:
        if 0 <= image_number < len(images):
            # Transform coordinates to target range and ensure they remain as floats
            x_transformed, y_transformed = transform_coordinates(x, y)
            # Clip coordinates to ensure they are within bounds
            x_clipped = np.clip(x_transformed, 0, images[image_number].shape[1] - 1)
            y_clipped = np.clip(y_transformed, 0, images[image_number].shape[0] - 1)
            heatmaps[image_number][int(y_clipped), int(x_clipped)] += heatmap_intensity

    images_with_heatmap = []

    for i, image in enumerate(images):
        heatmap = gaussian_filter(heatmaps[i], sigma=blur_radius)
        heatmap = (heatmap / heatmap.max()) * 255  # Normalize for visibility

        # Apply a cooler colormap (e.g., cv2.COLORMAP_WINTER or cv2.COLORMAP_COOL)
        color_heatmap = cv2.applyColorMap(heatmap.astype(np.uint8), cv2.COLORMAP_JET)
        
        # Adjust overlay weights for more transparency
        overlayed_image = cv2.addWeighted(image, 0.7, color_heatmap, 0.3, 0)  # More emphasis on original image
        images_with_heatmap.append(overlayed_image)

    return images_with_heatmap

if __name__ == "__main__":
    image_files = ["images/level0.png", "images/level1.png", "images/level2.png", "images/level3.png", "images/level4.png"]  # Replace with paths to your images in 'data/' folder
    images = [cv2.imread(file) for file in image_files]

    data_points = fetch_data_from_csv("data/analyticsDeath.csv")

    images_with_heatmap = apply_heatmap_to_images(images, data_points)

    for i, img in enumerate(images_with_heatmap):
        cv2.imwrite(f"data/deathAnalyticsForLevel{i}.png", img)