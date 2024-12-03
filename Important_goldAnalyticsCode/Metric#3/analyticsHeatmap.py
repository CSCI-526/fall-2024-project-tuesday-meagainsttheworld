import cv2
import numpy as np
import csv
import math
import matplotlib.pyplot as plt

def fetch_data_from_csv(file_path):
    data_points = []
    sessionIDSet = set()
    with open(file_path, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        for row in csvreader:
            if int(row['build']) < 6: continue
            level_number = int(row['currLevel'][5])
            x = float(row['playerX'])
            y = float(row['playerY'])
            data_points.append((level_number, x, y))
            sessionIDSet.add(row['sessionID'])
    sessionIDCount = len(sessionIDSet)
    return [data_points, sessionIDCount]

def transform_coordinates(x, y, x_start=-32, y_start=-18, x_target=2560, y_target=1440):
    x_transformed = ((x - x_start) * x_target / 64)
    y_transformed = 1440 - ((y - y_start) * y_target / 36)
    return x_transformed, y_transformed

def calculate_opacity(change_count):
    return -math.log10(change_count) * 2.1 + 2.2

# Apply grid-based painting with red fill and borders based on death counts
def apply_grid_painting(images, data_points, grid_size=(80, 45), max_intensity=255):
    images_with_grid = []

    for i, image in enumerate(images):
        height, width = image.shape[:2]
        grid_height = height // grid_size[1]
        grid_width = width // grid_size[0]

        grid_counts = np.zeros((grid_size[1], grid_size[0]), dtype=np.float32)

        for level_number, x, y in data_points:
            if level_number == i:  # Check if data point is for the current image
                x_transformed, y_transformed = transform_coordinates(x, y)
                col = int(np.clip(x_transformed / grid_width, 0, grid_size[0] - 1))
                row = int(np.clip(y_transformed / grid_height, 0, grid_size[1] - 1))
                grid_counts[row, col] += 1

        # Copy the original image to add grid overlay
        overlay_image = image.copy()

        # Draw filled color and borders based on death counts
        for row in range(grid_size[1]):
            for col in range(grid_size[0]):
                change_count = grid_counts[row, col]
                if change_count > 3:
                    grid_opacity = calculate_opacity(change_count)
                    if grid_opacity >= 0:
                        fill_color = (int(max_intensity * grid_opacity), 255, int(max_intensity * grid_opacity))  # Green fill
                    else:
                        fill_color = (0, int(max_intensity * grid_opacity + max_intensity), 0)  # Darker Green fill
                    top_left = (col * grid_width+2, row * grid_height+2)
                    bottom_right = ((col + 1) * grid_width-2, (row + 1) * grid_height-2)
                    cv2.rectangle(overlay_image, top_left, bottom_right, fill_color, thickness=-1)

        images_with_grid.append(overlay_image)

    return images_with_grid

def count_avg_gravity(sessionIDCount,data_points):
    level_count = 4
    avg_gravity = np.zeros(level_count)
    for level_number in data_points:
        avg_gravity[level_number[0]] += 1
    avg_gravity = [x / (2*sessionIDCount) for x in avg_gravity]
    return avg_gravity

def draw_graph(avg_gravity):
    # Define x-axis values as indices of avg_gravity (representing level numbers)
    level_numbers = range(len(avg_gravity))

    # Create a bar graph
    plt.figure(figsize=(10, 6))
    plt.bar(level_numbers, avg_gravity, color='forestgreen')

    # Add labels and title
    plt.xlabel("Level Number", fontsize=16)
    plt.ylabel("Average Number of Gravity Shifts", fontsize=16)
    plt.title("Average Number of Gravity Changes per Level in Mirror Mirror", fontsize=18)

    plt.xticks(ticks=range(len(avg_gravity)), fontsize=16)
    plt.yticks(fontsize=16)

    for i, value in enumerate(avg_gravity):
        plt.text(i, value + 0.1, f"{value:.1f}", ha='center', va='bottom', fontsize=16)

    # Display the plot
    plt.show()

if __name__ == "__main__":
    image_files = ["images/level0.png", "images/level1.png", "images/level2.png","images/level3.png"]  # Replace with paths to your images in 'data/' folder
    images = [cv2.imread(file) for file in image_files]

    [data_points,sessionIDCount] = fetch_data_from_csv("data/analyticsGravityChangePosition.csv")

    images_with_heatmap = apply_grid_painting(images, data_points)

    avg_gravity = count_avg_gravity(sessionIDCount,data_points)

    for i, img in enumerate(images_with_heatmap):
        cv2.imwrite(f"data/gravityChangeAnalyticsForLevel{i}.png", img)
    
    draw_graph(avg_gravity)