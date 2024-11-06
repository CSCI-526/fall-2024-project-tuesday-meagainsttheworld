# Create a list of average values (replace with your actual values)
averages <- c(1.25, 2.5, 3.75, 4.0, 2.25, 1.75, 3.0, 4.5)

par(mar = c(8, 4, 4, 2))

# Create labels for each section and level
sections <- c("Level 1 - Section 1", "Level 1 - Section 2", 
              "Level 2 - Section 1", "Level 2 - Section 2", 
              "Level 3 - Section 1", "Level 3 - Section 2", 
              "Level 4 - Section 1", "Level 4 - Section 2")

# Plot the bar chart
barplot(averages, names.arg = sections, 
 main = "Average Death for Section and Level for Mirror Mirror", 
        ylab = "Averages Dies", ylim=c(0,5), las=2, col="lightblue")
