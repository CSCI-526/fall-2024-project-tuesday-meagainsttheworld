install.packages("ggplot2")

# Load necessary libraries
library(ggplot2)

# Function to convert time from 'minutes:seconds' to total seconds
convert_to_seconds <- function(time_str) {
  parts <- unlist(strsplit(time_str, ":"))
  minutes <- as.numeric(parts[1])
  seconds <- as.numeric(parts[2])
  total_seconds <- minutes * 60 + seconds
  return(total_seconds)
}

# List of average times in 'minutes:seconds' format
average_times <- c("0:45", "0:26", "1:02", "0:35", "1:25")  # Modify this list with actual times
sections <- c("Section 1", "Section 2", "Section 3", "Section 4", "Section 5")  # Corresponding sections

# Convert all times to seconds
times_seconds <- sapply(average_times, convert_to_seconds)

# Create a data frame for plotting
data_frame <- data.frame(
  Section = sections,
  TimeSeconds = times_seconds
)

# Plot the bar chart
p <- ggplot(data_frame, aes(x = Section, y = TimeSeconds)) +
  geom_bar(stat = "identity", fill = "skyblue") +
  labs(x = "Section number of Level X", y = "Average Play Time (seconds)") +
  theme_minimal()

p + ggtitle("Average Play Time for Section and Level for Mirror Mirror")
