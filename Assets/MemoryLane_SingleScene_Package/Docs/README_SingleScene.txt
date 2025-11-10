Memory Lane – Single Scene Level System (Unity 6 / 6000.x compatible)
====================================================================

What you get
------------
- Single-scene gameplay flow using ScriptableObjects.
- LevelData: positions for path cubes, end tile index, per-cube or single color, and optional "falling" cube indices.
- LevelCatalogue: list of all LevelData assets (drag-and-drop in the inspector).
- LevelManager: spawns cubes/end tile, sets proper layers to match your blockMask/endMask, binds runtime arrays to the game manager.
- SingleSceneGameManager: a drop-in version of your GameManager that keeps your exact countdown → hide blocks → go flow, but loads next level without scenes.
- LevelUI: optional next/prev buttons for quick testing.

Unity version
-------------
Tested against Unity 6 (6000.x). Uses only standard API.

Setup (once)
------------
1) Create a new scene (e.g., GameScene) with:
   - An empty GameObject "Managers" → add components:
     • LevelManager
     • SingleSceneGameManager
   - A "LevelRoot" empty GameObject (assign to LevelManager.levelRoot).
   - Your Player prefab in the scene with SimpleGridMovement (assign to LevelManager.player). Place it on top of a starting cube position.
   - Optional: UI with countdownText, goImage, winImage assigned to SingleSceneGameManager.

2) Prefabs & Layers:
   - Create two prefabs:
     • cubePrefab (normal path cube)
     • endPrefab  (end tile; visually different)
   - Make sure their colliders exist.
   - Create layers for blocks and end (e.g., "Block" and "End") and in the LevelManager assign:
     • blockMask to "Block"
     • endMask   to "End"

3) ScriptableObjects:
   - Right-click in Project → Create → MemoryLane → Level Data (repeat to create as many levels as you want).
   - Fill each LevelData:
     • levelNumber (1-based display number)
     • cubePositions (array of world positions)
     • endIndex (which element in cubePositions is the END tile)
     • Optionally cubeColors (same length as positions) or just set a singleColor
     • Optionally fallingCubeIndices for the win animation.
   - Create a "LevelCatalogue" (Create → MemoryLane → Level Catalogue) and drag all LevelData assets in order.
   - Assign this catalogue to LevelManager.catalogue.

4) Wire-up references:
   - LevelManager: set cubePrefab, endPrefab, levelRoot, catalogue, player, gameManager, masks.
   - SingleSceneGameManager: set countdownText, goImage, winImage, and levelManager.
   - LevelUI: assign levelManager and buttons if you use it.

Flow
----
- LevelManager builds the level in Awake() and immediately binds arrays to SingleSceneGameManager.
- SingleSceneGameManager runs your 5→1 countdown, hides blocks, enables player.canMove.
- When the player reaches END, it waits until movement/fall complete, shows "win", plays falling cubes anim, unlocks next, then LevelManager.LoadNextLevel().

Keep your existing scripts
--------------------------
- SimpleGridMovement, music/SFX scripts, camera, etc. all stay the same.
- You no longer need per-level scenes (LVL 1, LVL 2, ...). Use one GameScene.

Notes
-----
- If you need per-level player start positions, add a "startPosition" field to LevelData; teleport player before countdown.
- If you want dynamic countdown length, add a "countdownSeconds" field to LevelData and pass it to SingleSceneGameManager.

Enjoy! :)
