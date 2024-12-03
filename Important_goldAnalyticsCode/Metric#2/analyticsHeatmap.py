import cv2
import numpy as np
import csv
import math

def fetch_data_from_csv(file_path):
    data_points = []
    with open(file_path, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        for row in csvreader:
            level_number = int(row['currLevel'][5])
            if level_number <= 2:
                if int(row['build']) < 6: continue
            x = float(row['playerX'])
            y = float(row['playerY'])
            waitTime = float(row['waitTime'])
            data_points.append((level_number, x, y, waitTime))
    return data_points

def transform_coordinates(x, y, x_start=-32, y_start=-18, x_target=2560, y_target=1440):
    x_transformed = ((x - x_start) * x_target / 64)
    y_transformed = 1440 - ((y - y_start) * y_target / 36)
    return x_transformed, y_transformed

def calculate_opacity(wait_count,i):
    if i > 0:
        return -math.log10(wait_count) * 1.5 + 1.65
    else:
        return -math.log10(wait_count) * 1.8 + 3.3

def apply_grid_painting(images, data_points, grid_size=(80, 45), max_intensity=255):
    images_with_grid = []

    for i, image in enumerate(images):
        height, width = image.shape[:2]
        grid_height = height // grid_size[1]
        grid_width = width // grid_size[0]

        grid_counts = np.zeros((grid_size[1], grid_size[0]), dtype=np.float32)

        for level_number, x, y, waitTime in data_points:
            if level_number == i:  # Check if data point is for the current image
                x_transformed, y_transformed = transform_coordinates(x, y)
                col = int(np.clip(x_transformed / grid_width, 0, grid_size[0] - 1))
                row = int(np.clip(y_transformed / grid_height, 0, grid_size[1] - 1))
                grid_counts[row, col] += waitTime

        # Copy the original image to add grid overlay
        overlay_image = image.copy()

        for row in range(grid_size[1]):
            for col in range(grid_size[0]):
                wait_count = grid_counts[row, col]
                if wait_count > 20 or (i > 0 and wait_count > 3):
                    grid_opacity = calculate_opacity(wait_count,i)
                    if grid_opacity >= 0:
                        fill_color = (255, int(max_intensity * grid_opacity), int(max_intensity * grid_opacity))  # Blue fill
                    else:
                        fill_color = (int(max_intensity * grid_opacity + max_intensity), 0, 0)  # Darker Blue fill
                    # Define color intensity based on calculated opacities

                    # Define cell boundaries
                    top_left = (col * grid_width+2, row * grid_height+2)
                    bottom_right = ((col + 1) * grid_width-2, (row + 1) * grid_height-2)

                    # Fill the grid cell with the calculated red intensity
                    cv2.rectangle(overlay_image, top_left, bottom_right, fill_color, thickness=-1)

        images_with_grid.append(overlay_image)

    return images_with_grid

if __name__ == "__main__":
    image_files = ["images/level0.png", "images/level1.png", "images/level2.png", "images/level3.png", "images/level4.png"]  # Replace with paths to your images in 'data/' folder
    images = [cv2.imread(file) for file in image_files]

    data_points = fetch_data_from_csv("data/analyticsWaitTime.csv")

    images_with_heatmap = apply_grid_painting(images, data_points)

    for i, img in enumerate(images_with_heatmap):
        cv2.imwrite(f"data/playPositionAnalyticsForLevel{i}.png", img)