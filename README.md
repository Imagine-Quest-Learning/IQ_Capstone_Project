# Enchanted Dugeon: dev branch
Dev branch is for game development including Assets, Packages, and ProjectSettings. 

Please work out of this branch for development needs. 

For online gameplay on GitHub pages, use the main branch.

## Code Organization
| Folder                       | Contents                      | Notes                                                                    |
| -----------------------------|:-----------------------------------------------------:|:------------------------------------------------:|
| .github/workflows            | PlayMode Test Automation File                         | A Github Action runs these tests on every commit |
| Assets/                      | All assets used in game development organized by game |                                                  |
| Assets/Scenes                | Front-End Work for each game                          | See below for more info                          |
| Assets/Scripts               | Back-End Logic for each game                          | See below for more info                          |
| Assets/Tests                 | Holds all tests written for game                      | See below for more info                          |
| Packages/ & ProjectSettings/ | Unity Editor Settings (used when pulling code)        |                                                  |

## Scene Organization
All Scenes are found in Assets/Scenes/...

When pushing code do not forget to add **meta data** (aka files with .unity.meta)

### Starting Page
```
startpage.unity
  * Contains start page (first thing that loads when game is started)

Development Information
  * StartPageManager.cs: Used to here to handle Start Button activation
```
### Introduction Page
```
IntroductionPage.unity
  * Contains the Instruction screens for players.
```
### Main Hall
```
MainHall.unity
  * Where the player spawns once the instructions are read.
  * Player uses the main hall to enter rooms (where other games are found)
  * Player returns to main hall after completing rooms

Development Information
  * Player.cs: Used here to spawn player
  * SceneChanger.cs: Used here to handle triggers into rooms
      * Each room has a BoxCollider2D setup over the door to notify SceneChanger when the Player wants to change rooms
  * GameManager.cs: Instance of GameManager starts here
```
### Subtraction Room
```
Subtraction.unity
  * The scene that loads if the player enters the first door from the left in the Main Hall.

Development Information
  * All scripts for this room found under Assets/Scripts/Subtraction
  * BarrelManager.cs: Manages the barrel prefabs in the scene
  * Bat.cs: Script attached to bat prefab used in the scene
  * BatDestroyZone.cs: Script attached to collider on far right of scene (to destroy bats if they get past the barrels)
  * BatSpawner.cs: Handles spawning bat prefabs in the scene
  * Chest.cs: Script attached to chest in the scene (handles player starting game)
  * InstructionsManager.cs: Handles the game instructions once player chooses to open chest
  * SubtractionGameManager.cs: Handles the core game logic once the player starts the game
  * SubtractionUtils.cs: Helper functions for SubtractionGameManager
```
### Addition Room
```
Scenes found under Assets/Scenes/Addition/...
  * The scene that loads if the player enters the middle room in the Main Hall.

Development Information

```
### Multiplication Room
```
Multiplication.unity
  * The scene that loads if the player enter the third door from left in the Main Hall.

Development Information

```
### Division Boss Battle 
```
Division.unity
  * The scene that loads if the player enter collides with the trigger on the far right of the Main Hall

Development Information
  * All scripts for this room found under Assets/Scripts/Division
  * MainGame.cs: Manages all the game logic for the room (generating/verifying questions, deducting player/golem health, etc.)
  * WinScreenController.cs: Controller to activate "win" screen
  * TimerController.cs: Script attached to the timer on the bottom of the question box to reset and count down the time (visual + logical)
  * PlayerHealth.cs: Script attached to the "Hearts" game object (in the Division.unity scene) with the functions to set/delete player's hearts (visual + logical)
  * HealthBar.cs: General health bar script that is attached to the golem with the functions to set/remove golem's health (visual + logical)
  * Golem.cs: Calls upon HealthBar.cs functions to set/remove the golem's health (logical)
```
## Setting Up Testing Locally 

### Installing Test Framework in Project
Install UTF via **Window > Package Manager**. Search for Test Framework under the **Unity Registry** in the Package Manager. Make sure to select latest version.

Once UTF is installed, open the **Packages/manifest.json** file with a text editor, and add a testables section after dependencies, like this:

```
,
"testables": [
"com.unity.inputsystem"
]
```

Save the file. This will be useful later on, when you’ll need to reference the Unity.InputSystem.TestFramework assembly for testing and emulating player input.

### Creating Tests Folder

With the root of your Project Assets folder highlighted, right-click and choose **Create > Testing > Tests Assembly Folder**.

This will create a Test folder, and inside should be a Test Assembly Definition called Tests.asmdef

### Creating Assembly Definition for Game Code

Make sure all of your game scripts are under a seperate folder in Assets. For example, mine are under Assets/Scripts/...

Right click the folder with all your scripts, in my case "Scripts/", then click **Create>Scripting>Assembly Definition**. Name the newly created file something like GameLogic. This should create a file called GameLogic.asmdef. Click on this file to open it in the Inspector and make sure **"Auto Referenced"** is checked.

### Setting up the Test.asmdef file
Go back to the Test.asmdef file (Assets/Tests/...) and click it to open it in Inspector. Under General Options, make sure only **"Override References"** is checked. 

Under Assembly Definition References add the following if not there:
> UnityEngine.TestRunner
>
> UnityEditor.TestRunner
>
> Unity.InputSystem
>
> Unity.InputSystem.TestFramework
>
> GameLogic

Where GameLogic is the name of the assembly definition file we created earlier for our game scripts. 

### Creating Your First Test File
Now you can go under Assets/Tests and create a new test file, something like LogicManagerTests.cs. 

You can find an example of how to setup your tests for my code in Assets/Tests/LogicManagerTests.cs

### Running Your Tests
Go to **Window>General>TestRunner** to run your tests. 

If you're getting **Null Exception Errors**, this normally means you're missing a reference to something in your test script.

Happy Testing!!!

### Testing Documentation Reference (Within Unity)
https://unity.com/how-to/automated-tests-unity-test-framework

## Setting up CI/CD Pipeline

### Github Action Workflow
To setup a CI/CD pipeline you need to create a .github/workflows/ folder at the root of your project.
Then, add a .yml file (Ex: unity_tests.yml)

### Github Secret Setup
Under **Repository Settings > Secrets** you'll need to create the following: \n
- UNITY_EMAIL (contains your unity email)
- UNITY_PASSWORD (contains your unity password)
- UNITY_LICENSE (contains the contents of your .ulf file)

To find your .ulf file please see https://game.ci/docs/github/activation.

If you're using a **personal license** it will be here:
```
Windows: C:\ProgramData\Unity\Unity_lic.ulf
Mac: /Library/Application\ Support/Unity/Unity_lic.ulf
Linux: ~/.local/share/unity3d/Unity/Unity_lic.ulf
```

### Workflow File Setup
Please see .github/workflows/unity_tests.yml for an exmaple of a workflow.
```
#Important Info

permissions => needed to allow github to run the action

the env portion is crucial for connecting Unity with Github!
env:
    UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
    UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
    UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
