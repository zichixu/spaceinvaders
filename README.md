# Space Shooter Game

## Description
Space Shooter is a 2D arcade-style game built using C# and the MonoGame framework. Players control a spaceship, maneuvering to avoid enemy ships while shooting them down to earn points. The game features increasing difficulty, a high score system, and dynamic enemy movement.

## Features
- Player-controlled spaceship with movement in all directions.
- Shooting mechanics with limited fire rate to prevent spamming.
- Enemy ships with basic and advanced movement patterns.
- Score tracking and a high score system.
- Lives system – lose all lives, and the game ends.
- Adjustable difficulty as the score increases.

## Controls
- **Arrow Keys**: Move the spaceship
- **Spacebar**: Fire a shot
- **Escape Key**: Exit the game

## Gameplay Mechanics
- Players start with **3 lives**.
- Destroying enemy ships increases the score.
- As the score increases, enemies spawn more frequently.
- If an enemy collides with the player, the player loses a life.
- The game ends when all lives are lost.
- At the end of the game, players can enter their name to record their high score.

## Installation
1. Ensure you have **MonoGame** installed.
2. Download the game files.
3. Open the project in **Visual Studio**.
4. Build and run the project.

## High Score System
- The game saves high scores in a file named `highscore.txt`.
- Only the top **10 scores** are retained.
- Players are prompted to enter their name when achieving a high score.
- High scores are displayed after the game ends.

## Requirements
- **.NET Framework / .NET Core** (compatible with MonoGame)
- **MonoGame Framework**
- **Windows OS** (or a compatible environment for MonoGame)

## Future Enhancements
- Add power-ups and different weapons.
- Implement sound effects and background music.
- Introduce different enemy types and boss fights.
- Improve graphics and animations.

## Credits
Developed by Eric Xu.

