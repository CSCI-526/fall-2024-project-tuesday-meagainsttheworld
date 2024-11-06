import cv2
import numpy as np
import gspread
from oauth2client.service_account import ServiceAccountCredentials
from scipy.ndimage import gaussian_filter
import matplotlib.pyplot as plt

# Google Sheets Setup
def fetch_data_from_google_sheets(sheet_url):
    scope = ["https://spreadsheets.google.com/feeds", "https://www.googleapis.com/auth/spreadsheets",
             "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/drive"]
    creds = ServiceAccountCredentials.from_json_keyfile_name("path_to_your_credentials.json", scope)
    client = gspread.authorize(creds)

    sheet = client.open_by_url(sheet_url).sheet1
    records = sheet.get_all_records()
    return [(record['currLevel'], record['playerX'], record['playerY']) for record in records]

# Transform coordinates from [-32, 32] to the target range
def transform_coordinates(x, y, x_range=(-32, 32), y_range=(-32, 32), x_target=(0, 500), y_target=(0, 300)):
    x_transformed = x_target[0] + ((x - x_range[0]) * (x_target[1] - x_target[0]) / (x_range[1] - x_range[0]))
    y_transformed = y_target[0] + ((y - y_range[0]) * (y_target[1] - y_target[0]) / (y_range[1] - y_range[0]))
    return int(x_transformed), int(y_transformed)

# Heatmap Application Function
def apply_heatmap_to_images(images, data_points, heatmap_intensity=1.0, blur_radius=10):
    heatmaps = {i: np.zeros((image.shape[0], image.shape[1])) for i, image in enumerate(images)}

    for image_number, x, y in data_points:
        if 0 <= image_number < len(images):
            # Transform coordinates
            x_transformed, y_transformed = transform_coordinates(x, y)
            # Ensure coordinates are within image bounds
            x_transformed = np.clip(x_transformed, 0, images[image_number].shape[1] - 1)
            y_transformed = np.clip(y_transformed, 0, images[image_number].shape[0] - 1)
            heatmaps[image_number][y_transformed, x_transformed] += heatmap_intensity

    images_with_heatmap = []

    for i, image in enumerate(images):
        heatmap = gaussian_filter(heatmaps[i], sigma=blur_radius)
        heatmap = (heatmap / heatmap.max()) * 255

        color_heatmap = cv2.applyColorMap(heatmap.astype(np.uint8), cv2.COLORMAP_JET)
        overlayed_image = cv2.addWeighted(image, 0.7, color_heatmap, 0.3, 0)
        images_with_heatmap.append(overlayed_image)

    return images_with_heatmap

if __name__ == "__main__":
    sheet_url = "yourhttps://docs.google.com/forms/d/1owDArsOzkEXDF_f-A1WlVFElIROFpD7tG30EBUEeKy0/edit?ts=672992dd"
    
    # Load images
    image_files = ["images/level0.png", "images/level1.png", "images/level2.png", "images/level3.png"]  # Replace with your image paths
    images = [cv2.imread(file) for file in image_files]

    # Fetch data points from Google Sheets
    data_points = fetch_data_from_google_sheets(sheet_url)

    # Apply heatmap and save/display results
    images_with_heatmap = apply_heatmap_to_images(images, data_points)

    for i, img in enumerate(images_with_heatmap):
        cv2.imwrite(f"output_image_{i}.png", img)