install.packages("ggplot2")

# Load necessary libraries
library(ggplot2)

# Function to convert time from 'minutes:seconds' to total seconds
convert_to_seconds <- function(time_str) {
  parts <- unlistsplit(time_str, ":"))
  minutes <- as.numeric(parts[1])
  seconds <- as.numeric(parts[2])
  total_seconds <- minutes * 60 + seconds
  return(total_seconds)
}

# List of average times in 'minutes:seconds' format
average_times <- c3:45", "4:20",2:30", "5:10", "3:15")  # Modify this list with actual
sections <- c("Section 1", "Section 2", "Section 3", "Section 4", "Section 5")  # Corresponding sections

# Convert all times to seconds
times_seconds <- sapply(average_times, convert_to_seconds)

# Create a data frame for plotting
data data.frame(
  Section = sections,
  TimeSeconds = times_seconds
)

# Plot the bar chart
ggplot(data, aes(x = Section, y = TimeSeconds)) +
  geom_bar(stat = "identity", fill = "skyblue") +
  labs(x = "Section number of Level X", y = "Averages Play Time (seconds)") +
  theme_minimal()

