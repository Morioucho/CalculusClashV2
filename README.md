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
> **When you run the game, ensure that you are using Alt + Enter to ensure that you are in full screen! If you are already in full screen, you do not need to do this. This may be fixed in another update but we were unable to complete this feature due other classes finals getting in the way.**

**If you wish to play the game, you can download the latest release from the [releases](https://github.com/Morioucho/CalculusClashV2/releases) page.** You will have to download the `.zip` file and then right click and unzip to create a folder for the game. Following that, you can run the `CalculusClash.exevX.X.X` (where X is the version number) in the folder. 

You may get a popup from the **Windows Defender Smartscreen** when trying to run the game, in order to get past this, you'll have to hit the `More Info` button and then `Run Anyways`. Windows Defender Smartscreen is software implemented by Windows that checks how new an executable is, it will automatically trigger for new programs such as this one.

This game is relatively simple to play, you can use WASD or the arrow keys in order to move across rooms. In order to progress through dialogue, you will have to use the `Enter` key. You may press the `Enter` key in dialogue to skip the typewriter effect that plays out.

# Architecture
> [!TIP]
> If you would like a better description of each Class, Enum, Interface, and Abstract Class, you may want to refer to all the code [here](https://github.com/Morioucho/CalculusClashV2/tree/main/Assets/Scripts).

This game is made in C# using the Unity library to design the game. The game has separate packages that each manage their own concern. The game also depends on other libraries for critical features.

The game is made using `Scenes`, where each scene is a part of the map. For example, a certain structure may have multiple Scenes, with 1 scene for each room.

The UI, such as the menu UI, pause UI, and battle UI, are also all in their own respective scenes. Most of the resources that we use for the game are under the Resources folder, with only a few being outside of that folder. For example, the Resources folder houses the `Audio`, `Enemy`, `EnemySprite`, etc. folders. These assets are **loaded at Runtime** and are in the Resources folder. If you have a resource that does not need to be modularly loaded or loaded at runtime, you can keep it in a folder at the same level as the Resources folder.

# Build
> [!CAUTION]
> If you wish to build the project from scratch, you must have a compatible version of Unity installed. If you do not have any version of Unity, you can install one from [here](https://unity.com/download).

If you wish to build this project, you must ensure that you have **Unity 6.1** or higher. This game is made in the 2D URP engine and should be able to run on any device.

The Unity Editor is available on both MacOS and Windows, and any computer should be able to run it. The game is also lightweight, so there isn't much concern about hardware requirements.

# Make Your Own
If you want to make your own game for your Calculus BC final, we have a few tips for your game. Most of these tips sound very obvious but when you're making the game you're bound to forget or overlook a few of them.

- **Use a consistent naming scheme.**
  - This helps mainly for programming the game, naming variables, scenes, and files in a consistent way helps with debugging and play testing.
- **Keep all entities in one folder, do NOT spread it out.**
  - This is a major tip that we failed to use in our own program, we made the mistake of spreading out our enemy data into multiple unneeded folders. For example, for the **Lagrange Demon**, we have it's data split across 3 folders: `Enemy`, `EnemySprite`, and `Dialogue`. This is a major issue as it makes playtesting harder than it needs to be and is not very intuitive. If we instead made a single `Enemy` folder and a folder for the Lagrange Demon with the relevant `.json` files inside, we would have done much better. 
- **Communicate with your team.**
  - We didn't communicate properly for the project at the start, and only truly started collaborating near the end of the project. Should we have started collaborating a bit better we could have made a lot more progress.
- **Only make a game if you have some experience.**
  - We were able to make a game since all of our members had experience with game development. If we were all new to game development, we would have not nearly as much progress as we did.
- **Create an outline before making the game.**
  - Before you make your game, be sure to outline each idea and split it into smaller tasks. This makes the game more achievable and more attainable.

# Credits
Programmed by Saumil Sharma, Levels designed by Om Kasar, Animation and Pixel Art by Christian Hsu, Math by Kason Lai.
