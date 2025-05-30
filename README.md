<p align="center">
    <img src="docs/images/CalculusClashLogo.png" width="400px" alt="Calculus Clash Logo">
</p>

> Calculus Clash is a game written in C# and Unity as our final project for AP Calculus BC. Calculus Clash is a RPG game where you fight different calculus-themed bosses.

# Features
- Questions from all 10 units of AP Calculus BC
- Different Calculus-themed bosses
- A story behind the game
- Multi-platform support (Windows, MacOS, and Linux)
- Controller support

# Play
> [!IMPORTANT]
> The game is not tested on platforms other than Windows, such as macOS or your web browser. There is no gurantee that the game will run as intended.

> [!CAUTION]
> **When you run the game, ensure that you are using Alt + Enter to ensure that you are in full screen! This game was made in way shorter time than given as our other teachers such as Girvan and Brittain decided to also give us finals colliding with this. As a result, you'll have to use Alt + Enter to run it.**

**If you wish to play the game, you can install the game from the [releases](https://github.com/Morioucho/CalculusClashV2/releases) page.** You will have to download the `.zip` file and then right click and unzip to create a folder for the game. Following that, you can run the `CalculusClash.exe` in the folder. The keys that you have to use will be similar to other games with this genre.

# Architecture
> [!TIP]
> If you would like a better description of each Class, Enum, Interface, and Abstract Class, you may want to refer to all the code [here](https://github.com/Morioucho/CalculusClashV2/tree/main/Assets/Scripts).

This game is made in C# using the Unity library to design the game. The game has separate packages that each manage their own concern. The game also depends on other libraries for critical features.

The game is made using `Scenes`, where each scene is a part of the map. For example, a certain structure may have multiple Scenes, with 1 scene for each room.

The UI, such as the menu UI, pause UI, and battle UI, are also all in their own respective scenes. Other resources such as textures, sounds, and music, are found in the `/textures/`, `/sounds/`, and `/music/` folders, respectively.

# Build
> [!CAUTION]
> If you wish to build the project from scratch, you must have a compatible version of Unity installed. If you do not have any version of Unity, you can install one from [here](https://unity.com/download).

If you wish to build this project, you must ensure that you have **Unity 6.1** or higher. This game is made in the 2D URP engine and should be able to run on any device.

The Unity Editor is available on both MacOS and Windows, and any computer should be able to run it. The game is also lightweight, so there isn't much concern about hardware requirements.

# Credits
Programmed by Saumil Sharma, Levels designed by Om Kasar, Animation and Pixel Art by Christian Hsu, Math by Kason Lai.
