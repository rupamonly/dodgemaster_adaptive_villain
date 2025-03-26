
# 🕹️ dodgemaster_adaptive_npc

This project is a 2D Unity game where a villain learns to dodge incoming balls using a single-layer perceptron. The perceptron is a simple AI model that takes 4 inputs — ball type, distance, speed, and success of the last dodge — and predicts whether the villain should dodge or stay still.

## How It Works:
- Initially, the villain does not dodge and takes hits from damaging balls (Red/Blue).

- After being hit by damaging balls, the villain "learns" to dodge using data collected from those encounters.

- The perceptron dynamically updates its weights based on whether the villain successfully dodges or fails.

- The villain is given a kickstart after some hits to trigger dodging behavior and then the perceptron fine-tunes itself with every new encounter, gradually enhancing dodge accuracy.


## 🚀 Features

🎮 Player Movement and Shooting

- Control the player using W, A, S, D keys to move:

- W – Move Up, A – Move Left, S - Move Down, D - Move Right
  
- Press Spacebar to shoot the selected ball in the direction the player is facing.

🎯 Ball Types and Selection:

- 🟢 Green Ball – No Damage (No dodge)

- 🔴 Red Ball – Damage (Triggers learning and dodge)

- 🔵 Blue Ball – Damage (Triggers learning and dodge)

- Press G for Green, R for Red, and B for Blue to select the ball.

- Press Spacebar to shoot the selected ball.

🧠 AI-Driven Villain Dodging:

- Villain initially takes hits from damaging balls and learns after multiple hits.

- Dynamically decides whether to dodge or not using a single-layer perceptron.

- Perceptron predicts dodge success based on ball type, distance, speed, and previous dodge outcomes.

🕹️ Collision and Training Logic:

- Detects collision with different ball types.

- Updates perceptron weights after each successful or failed dodge.

## ⚙️ Requirements to Run This Project ⚙️

🖥️ Unity Version

- Unity 2021.3 or higher (Recommended for stability and compatibility).

💾 Dependencies/Packages

- 2D Sprite Package (Installed by default in 2D projects).

- Rigidbody2D and Collider Components attached to GameObjects.

📂 Project Setup

- Import all necessary assets, including:

- Ball Prefabs: Green, Red, and Blue.

- Player Prefab with attached BallSpawner script.

- Villain Prefab with attached VillainController script.

🧠 Physics Settings

- Enable 2D Physics with appropriate collision layers for ball and villain interactions.

🔥 Perceptron Logic

- Ensure Perceptron.cs is correctly linked to VillainController.cs for AI-driven learning and adaptation.

🚀 Build & Run Environment

- Platform: PC (Windows/Mac/Linux)

  ## 🛠️ Technologies Used
- Unity 2022.3.40f1 – Game engine for developing the 2D game.

- C# – Programming language used for implementing game logic and AI behavior.

- Photopea – Online image editor used to create and edit game assets, including sprite-based text for UI.

- Kenney Assets – Free game asset package used for creating the villain’s sprites and animations.

- GitHub – Version control and repository hosting for project management and collaboration.

- GitHub Desktop – GUI-based version control tool used for managing commits and pushing changes to the repository.

- ShaderLab & HLSL – Used implicitly by Unity for rendering graphics and managing shaders.


## 📥 Clone and Run

```
# Clone the repo
https://github.com/rupamonly/dodgemaster_adaptive_villain.git

# Move to project directory
cd dodgemaster_adaptive_villain

```
🎮 Open in Unity
- Open Unity Hub -> Click on Open -> Select the cloned project folder

▶️ Run the Game
- Click File → Build Settings

- Set the target platform to PC, Mac & Linux Standalone (or any desired platform)

- Click Play in the Unity Editor to run the game


## 📚 Usage Instructions

Unity Version Compatibility
- Ensure that Unity 2022.3.41f LTS or any other version is installed.

Auto-Generated Folders and Files
- ⚠️ Important: Several folders and files are not included in the repository due to their large size or auto-generated nature. Unity will regenerate these automatically when you open the project. These include:

- Library – Stores cached and temporary data.

- Logs – Contains logs for debugging and performance tracking.

- ProjectSettings – Contains project-specific settings (input mappings, quality settings, etc.).

- UserSettings – Stores user-specific settings that do not affect other developers.

- .vs, .sln, and Assembly-CSharp.csproj – Generated for Visual Studio or other IDEs.

Regenerating Missing Files
- When you open the project in Unity:

- Unity will automatically create the missing folders (Library, - Logs, etc.) based on the current project configuration.


## ⚠️ Limitations
⚠️ Single-Layer Perceptron

- The system uses a basic perceptron with limited learning capacity. It is unable to handle non-linearly separable data, which limits its ability to handle complex scenarios.

⚠️ Limited Input Features

- The perceptron only considers four inputs: ball type, distance, speed, and previous dodge success. This restricts its decision-making capabilities and overlooks additional factors that could enhance performance.

⚠️ No Persistent Memory

- The perceptron resets after every game session, meaning the learning progress is lost when the game restarts. Persistent learning or model saving is not yet implemented.

⚠️ Random Dodge Direction

- The villain’s dodge direction is randomized (left or right), which may lead to inconsistent or suboptimal dodge behavior.

## 📈 Future Scope
- Upgrade to Multi-Layer Perceptron (MLP) to handle more complex decision-making scenarios.

- Implement persistent learning to retain perceptron training across game sessions.

- Add the ability to track and process multiple objects for more realistic gameplay.

- Introduce an optimized dodge algorithm to determine the most effective direction dynamically.

- Enhance visual effects and animations to improve the overall gaming experience.



## 📜 License 

This project is licensed under the MIT License.

- It was developed for **educational purposes** only.

- Please refrain from contributing to the repository, but feel free to clone and modify it locally for learning and experimentation.












[![License: MIT](https://img.shields.io/badge/License-MIT-gree.svg)](./LICENSE)

