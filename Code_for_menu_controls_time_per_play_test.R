install.packages("ggplot2")

# Load necessary library
library(gg2)

# Example data: time from loading the game to start moving (in seconds)
# Modify this list with your actual data
times_from_load <- c(15, 18, 12, 20, 14, 16, 19)  # Time for each playtest
playtests <- c("Playtest 1", "Playtest 2", "Playtest 3", "Playtest 4", "Playtest 5", "Playtest 6", "Playtest 7")

# Create a data frame for plotting
data <- data.frame(
  Playtest = playtests,
  TimeInSeconds = times_from_load
)

# Plot bar chart
ggplot(data, aes(x = Playtest, y = TimeInSeconds)) +
  geom_bar(stat = "identity", fill = "skyblue") +
  labs(x = "Playtest Number", y = "Time from Loading to Start Moving (seconds)") +
  theme_minimal()
