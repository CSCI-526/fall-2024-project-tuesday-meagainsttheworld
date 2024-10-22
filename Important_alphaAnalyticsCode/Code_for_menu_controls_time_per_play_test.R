install.packages("ggplot2")
install.packages("reshape2")

# Define the series data
loading_time <- c(11, 10, 11)    # Loading time in seconds for each play test
menu_reading_time <- c(20, 22, 5)  # Menu reading time in seconds
game_controls_time <- c(40, 35, 10)  # Game controls time in seconds

# Combine the data into a data frame for easy plotting
data <- data.frame(
  Play_Test = c("1st Play Test", "2nd Play Test", "3rd Play Test"),
  Loading_Time = loading_time,
  Menu_Reading_Time = menu_reading_time,
  Game_Controls_Time = game_controls_time
)

# Load ggplot2 for plotting
library(ggplot2)

# Melt the data for easier plotting with ggplot2
library(reshape2)
melted_data <- melt(data, id.vars = 'Play_Test')

# Create the bar plot
p <- ggplot(melted_data, aes(x = Play_Test, y = value, fill = variable)) +
  geom_bar(stat = "identity", position = "dodge") +
  labs(x = "Playtest # in level X", 
       y = "Time in seconds", 
       fill = "Time Series") +
  theme_minimal()

p + ggtitle("Loading, Menu, and Game Control Time in a certain level X")