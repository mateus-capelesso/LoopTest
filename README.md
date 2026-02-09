# Infinity Loop - Unity Puzzle Game

A Unity-based puzzle game where players rotate node pieces to connect power sources to targets, creating complete circuits. The game features a clean MVC architecture, persistent progression, and analytics integration.

## Architecture Overview

### MVC Pattern

The project follows the **Model-View-Controller (MVC)** pattern for its core components:

#### Node System (`Assets/_Project/Scripts/Node/`)
- **NodeModel** - Stores node data (type, shape, direction, powered state)
- **NodeView** - Handles visual representation and animations using DOTween
- **NodeController** - Manages logic, rotation, neighbor connections, and power flow

#### Game System (`Assets/_Project/Scripts/Game/`)
- **GameModel** - Tracks score, current level, and max unlocked level
- **GameView** - UI management through state machine integration
- **GameController** - Orchestrates level loading, progression, and game flow

This separation ensures clean code organization and makes the system easy to extend and maintain.

### AudioController Singleton

The `AudioController` (`Assets/_Project/Scripts/Audio/AudioController.cs`) is implemented as a **Singleton** pattern:

```csharp
private static AudioController _instance;
public static AudioController Instance => _instance;

void Awake() {
    if (_instance != null && _instance != this) {
        Destroy(gameObject);
        return;
    }
    _instance = this;
    DontDestroyOnLoad(gameObject);
}
```

This allows any component in the game to access audio functionality via `AudioController.Instance.PlaySoundEffect(AudioType.Press)` without needing direct references. The singleton persists across scene loads.

## Node Connection Logic

### Direction System

The project uses a `Direction` enum (Up, Right, Down, Left) mapped to indices 0-3. This simplifies rotation calculations:

```csharp
public enum Direction {
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
```

Rotation is achieved by incrementing the direction index modulo 4, and angles are calculated as `-360/4 * index`.

### Bitwise Shape Encoding

Node shapes are encoded as **4-bit binary values** (bytes) where each bit represents a connection in a direction:

| Node Type | Binary | Decimal | Connections |
|-----------|--------|---------|-------------|
| Line      | 1010   | 10      | Up + Down |
| L-Shape   | 1100   | 12      | Up + Right |
| T-Shape   | 1110   | 14      | Up + Right + Down |
| Cross     | 1111   | 15      | All sides |
| Source    | 1000   | 8       | Up (output) |
| Target    | 1000   | 8       | Up (input) |

The `HasOutput()` method in `NodeController.cs:52` uses bitwise operations to check if a node has a connection in a specific direction:

```csharp
public bool HasOutput(Direction dir) {
    int sideIndex = ((int)dir - (int)_model.direction + _directionCount) % _directionCount;
    byte checkMask = (byte)((1 << (_directionCount - 1)) >> sideIndex);
    return (_model.baseShape & checkMask) != 0;
}
```

This calculates the relative direction accounting for the node's current rotation, then checks if the corresponding bit is set in the base shape.

## BFS Win Condition Algorithm

The `BFSController` (`Assets/_Project/Scripts/Graph/BFSController.cs`) implements a **Breadth-First Search** algorithm to propagate power through the graph:

1. **Initialization** - Reset all nodes (only sources are powered)
2. **Queue Setup** - Enqueue all source nodes
3. **Traversal** - For each node in the queue:
   - Check all 4 directions for outputs using `HasOutput()`
   - Get neighbor in that direction
   - Verify the neighbor has a matching input (`HasOutput(oppositeDirection)`)
   - If connected, power the neighbor and enqueue it
4. **Win Check** - After BFS completes, verify all target nodes are powered

The algorithm runs every time a player rotates a piece (`GraphController.cs:149`), immediately updating the power state and checking for victory.

## Data Management with ScriptableObjects

All game configuration data is stored in **ScriptableObjects** in the `Assets/_Project/Data/` folder:

- **GameConfig** - Global settings (user key, points per piece, level array)
- **LevelData** - Level-specific data (prefab reference, cell size, level name)
- **AudioConfig** - Audio clip mappings for different sound types
- **NodePrefabs** - Prefab references for each node type

This approach makes data easily editable in the Unity Inspector without code changes and enables asset reusability.

## UI State Machine

The `ViewStateMachine` (`Assets/_Project/Scripts/View/ViewStateMachine.cs`) manages UI transitions:

- **States**: MainMenu, LevelSelect, InGame, EndGame
- **Dictionary Mapping**: Serialized list maps ViewState enums to BaseView components
- **Transitions**: `ChangeState()` handles entering/exiting views cleanly
- **Events**: Bridges UI interactions to GameController (OnPlayButtonPressed, OnLevelSelected, etc.)

This keeps UI logic decoupled from game logic and makes adding new screens straightforward.

## Persistent Data System

The `GamePersistentData` class (`Assets/_Project/Scripts/Game/GamePersistentData.cs`) handles save/load operations:

- **Storage**: Uses Unity's `PlayerPrefs` for persistent storage
- **Serialization**: Newtonsoft.Json serializes `GameModel` to/from JSON
- **Data Tracked**: Player's max unlocked level and total score
- **Auto-save**: Progress is saved automatically when completing a new level (`GameController.cs:98`)

## Creating New Levels

### Step-by-Step Guide

1. **Create Scene Setup**
   - Create an empty GameObject in your scene to hold the level

2. **Add Node Builders**
   - Drag builder prefabs from `Assets/_Project/Prefabs/Builder/` into the level GameObject:
     - `LineBuilder.prefab` - Straight connection
     - `LShapeBuilder.prefab` - Corner connection
     - `TShapeBuilder.prefab` - T-junction connection
     - `CrossBuilder.prefab` - 4-way intersection
     - `SourceBuilder.prefab` - Power source (required)
     - `TargetBuilder.prefab` - Power target (required)

3. **Position Nodes**
   - Snap nodes to grid positions (default grid size is 1 unit)
   - Use Unity's vertex snapping (hold V and drag) for precise placement

4. **Set Node Directions**
   - Select each NodeBuilder in the Inspector
   - Change the `Direction` field (Up, Right, Down, Left)
   - The piece will **automatically rotate** in the editor to match
   - This Direction enum simplifies all rotation logic in gameplay

5. **Save as Prefab**
   - Drag the level GameObject into `Assets/_Project/Prefabs/Levels/`
   - This creates a reusable prefab

6. **Create LevelData Asset**
   - Right-click in `Assets/_Project/Data/Levels/`
   - Create → InfinityLoop → Level Prefab Data
   - Set the `Level Name`
   - Drag your level prefab into the `Level Prefab` slot
   - Adjust `Cell Size` if using non-standard grid spacing

7. **Add to Game**
   - Open the GameConfig asset (`Assets/_Project/Data/GameConfig/`)
   - Add your new LevelData to the `Levels` array
   - Levels are unlocked sequentially based on array order

### Builder Prefabs

Each builder prefab has a `NodeBuilder` component that stores:
- **Type Id**: The node type (Line, LShape, Source, etc.)
- **Direction**: Initial rotation using the Direction enum

The `GraphController` processes these builders at runtime, instantiating the actual node prefabs and establishing neighbor connections automatically.

## Dependencies

The project uses the following third-party libraries to simplify development:

- **[DOTween](http://dotween.demigiant.com/)** - Smooth animations for node rotations and UI transitions
- **[Newtonsoft.Json](https://www.newtonsoft.com/json)** - JSON serialization for save data
- **[Amplitude SDK](https://amplitude.com/)** - Analytics and event tracking

## Future Improvements

### 1. Sprite Atlas Management
**Current Issue**: Individual sprite files are loaded separately, causing draw call overhead.

### 2. Threaded Analytics
**Current Issue**: Analytics calls run on the main thread, potentially causing frame drops.

### 3. Multitouch Support
**Current Issue**: Game only responds to single touch/click events.

## Project Structure

```
Assets/_Project/
├── Audio/              # Audio clip files
├── Data/               # ScriptableObject assets
│   ├── Audio/          # AudioConfig assets
│   ├── GameConfig/     # GameConfig asset
│   ├── Levels/         # LevelData assets
│   └── Nodes/          # NodePrefabs assets
├── Materials/          # Material assets
├── Prefabs/
│   ├── Builder/        # NodeBuilder prefabs for level design
│   ├── Levels/         # Level prefabs
│   ├── Nodes/          # Runtime node prefabs
│   └── UI/             # UI prefabs
├── Scripts/
│   ├── Analytics/      # AnalyticsWrapper, AmplitudeSDK integration
│   ├── Audio/          # AudioController, AudioConfig
│   ├── Game/           # GameController, GameModel, GameView, GameConfig
│   ├── Graph/          # GraphController, BFSController
│   ├── Level/          # LevelData
│   ├── Node/           # NodeController, NodeModel, NodeView, NodeBuilder
│   └── View/           # ViewStateMachine, BaseView implementations
└── Sprites/            # Sprite assets
```

## Getting Started

1. Open the project in Unity (6.2 or later recommended)
2. Load MainScene
3. Press Play to test the game
4. Create new levels following the guide above
5. Configure GameConfig asset to adjust difficulty and progression

---