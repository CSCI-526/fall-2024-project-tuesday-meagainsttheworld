import pandas as pd
import matplotlib.pyplot as plt

# id = "1XZOLOrfTeGfHMh3jzVQfaGetIGEY8Gpr-hwpYtoMTQY"

# Loading data
data = pd.read_csv('gameData.csv')

# Ignoring NA
death_data = data[data['playerDead'].notna()]

# Counting Deaths
death_counts = death_data.groupby(['currLevel', 'playerDead']).size().reset_index(name='deaths')

# Unique Sessions in each level
session_counts = data.groupby('currLevel')['sessionID'].nunique().reset_index(name='unique_sessions')

# Merging
merged_data = pd.merge(death_counts, session_counts, on='currLevel')

# Calculating avg deaths
merged_data['avg_deaths_per_session'] = merged_data['deaths'] / merged_data['unique_sessions']

# Pivoting for plotting
pivot_data = merged_data.pivot(index='currLevel', columns='playerDead', values='avg_deaths_per_session').fillna(0)

pivot_data.plot(kind='bar', figsize=(12, 8))

plt.xlabel('Levels')
plt.ylabel('Average Number of Deaths per Session')
plt.title('Average Number of Deaths per Player per Session per Level')

plt.legend(title='Player')

# plt.show()

plt.savefig('my_plot.png')