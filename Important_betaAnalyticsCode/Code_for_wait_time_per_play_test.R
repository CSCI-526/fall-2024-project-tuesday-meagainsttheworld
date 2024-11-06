install.packages("ggplot2")

# Load necessary library
library(ggplot2)

# Example data: wait times for each player (in seconds)
# Modify this list according to your actual data
black_player_times <- c(35,23,45,1,10)  # Wait times for black player
white_player_times <- c(6,21,2,33,29)  # Wait times for white player
playtests <- c("Playtest 1", "Playtest 2", "Playtest 3", "Playtest 4", "Playtest 5")

# Create a data frame for both series
data <- data.frame(
  Playtest = rep(playtests, 2),  # Replicate playtest names for both series
  WaitingTime = c(black_player_times, white_player_times),
  Player = rep(c("Black Player", "White Player"), each = length(playtests))  # Label the series
)

# Plot bar chart with both series
p <- ggplot(data, aes(x = Playtest, y = WaitingTime, fill = Player)) +
  geom_bar(stat = "identity", position = "dodge") +  # 'dodge' positions bars side-by-side
  labs(x = "Playtest Number of Level 2 of Mirror Mirror", y = "Waiting Time (seconds)") +
  scale_fill_manual(values = c("blue", "red")) +  # Custom colors for players
  theme_minimal()

p + ggtitle("Total wait time for each player in a certain level")