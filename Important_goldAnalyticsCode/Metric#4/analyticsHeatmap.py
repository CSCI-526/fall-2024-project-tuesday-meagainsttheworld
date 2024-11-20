import cv2
import numpy as np
import csv
import math

def fetch_data_from_csv(file_path):
    level_count = 3
    data_points = [{} for _ in range(level_count)]
    with open(file_path, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        for row in csvreader:
            if int(row['build']) < 5: continue
            level_number = int(row['currLevel'][5])
            pillLocation = row['pillLocation']
            isStart = 1 if row['isStart'] == 'True' else 0
            x = float(row['playerX'])
            y = float(row['playerY'])
            if(math.sqrt((float(eval(pillLocation)[0])-x)**2+(float(eval(pillLocation)[1])-y)**2)) < 1.5 and isStart == 1: continue
            if pillLocation not in data_points[level_number]:
                data_points[level_number][pillLocation] = []  # Initialize the list for this pillLocation
            else:
                data_points[level_number][pillLocation].append((isStart, x, y))
    return data_points

def transform_coordinates(x, y, x_start=-32, y_start=-18, x_target=2560, y_target=1440):
    x_transformed = ((x - x_start) * x_target / 64)
    y_transformed = 1440 - ((y - y_start) * y_target / 36)
    return x_transformed, y_transformed

def calculate_opacity(frequency_count):
    return -math.log10(frequency_count) * 1.5 + 0.9

# Apply grid-based painting with red fill and borders based on death counts
def apply_grid_painting(images, data_points, grid_size=(80, 45), max_intensity=255):
    images_with_grid = [{} for _ in range(len(data_points))]

    for i, image in enumerate(images):
        height, width = image.shape[:2]
        grid_height = height // grid_size[1]
        grid_width = width // grid_size[0]

        for key, pillDict in data_points[i].items():
            grid_counts = np.zeros((grid_size[1], grid_size[0], 2), dtype=np.float32)

            for isStart, x, y in pillDict:
                x_transformed, y_transformed = transform_coordinates(x, y)
                col = int(np.clip(x_transformed / grid_width, 0, grid_size[0] - 1))
                row = int(np.clip(y_transformed / grid_height, 0, grid_size[1] - 1))
                grid_counts[row, col, isStart] += 1

            # Copy the original image to add grid overlay
            overlay_image = image.copy()

            # Draw filled color and borders based on death counts
            for row in range(grid_size[1]):
                for col in range(grid_size[0]):
                    frequency_count = grid_counts[row, col]
                    if frequency_count[0] > 0:
                        grid_opacity = calculate_opacity(frequency_count[0])
                        if grid_opacity >= 0:
                            fill_color = (int(max_intensity * grid_opacity), int(max_intensity * grid_opacity), 255)  # Red fill
                        else:
                            fill_color = (0, 0, int(max_intensity * grid_opacity + max_intensity))  # Darker Red fill
                        top_left = (col * grid_width+2, row * grid_height+2)
                        bottom_right = ((col + 1) * grid_width-2, (row + 1) * grid_height-2)
                        cv2.rectangle(overlay_image, top_left, bottom_right, fill_color, thickness=-1)
                    if frequency_count[1] > 0:
                        grid_opacity = calculate_opacity(frequency_count[1])
                        if grid_opacity >= 0:
                            fill_color = (int(max_intensity * grid_opacity), 255, int(max_intensity * grid_opacity))  # Green fill
                        else:
                            fill_color = (0, int(max_intensity * grid_opacity + max_intensity), 0)  # Darker Green fill
                        top_left = (col * grid_width+2, row * grid_height+2)
                        bottom_right = ((col + 1) * grid_width-2, (row + 1) * grid_height-2)
                        cv2.rectangle(overlay_image, top_left, bottom_right, fill_color, thickness=-1)
            center = transform_coordinates(eval(key)[0],eval(key)[1])
            center = (int(center[0]),int(center[1]))
            outer_radius = 10
            cv2.circle(overlay_image, center, outer_radius, (0, 0, 255), -1)  # Fill red

            images_with_grid[i][key] = overlay_image

    return images_with_grid

if __name__ == "__main__":
    image_files = ["images/level0.png", "images/level1.png", "images/level2.png"]  # Replace with paths to your images in 'data/' folder
    images = [cv2.imread(file) for file in image_files]

    data_points = fetch_data_from_csv("data/analyticsSizeChangingPills.csv")

    images_with_heatmap = apply_grid_painting(images, data_points)
    for i, val in enumerate(images_with_heatmap):
        count = 0
        for key, img in images_with_heatmap[i].items():
            count += 1
            cv2.imwrite(f"data/sizeChangingPillAnalyticsForLevel{i}Pill{count}.png", img)