# Pirate Trade Sim — Phase 1: Foundation

## 1. Core Architecture
- [x] Create folder structure: Assets/Scripts/Core, Assets/Scripts/Economy, Assets/Scripts/Navigation, Assets/Scripts/UI.
- [x] Implement ShipStats.cs in PirateGame.Core namespace. Fields: gold (int), baseSpeed (float), cargoCapacity (int). Use [SerializeField].
- [x] Create ItemData.cs as a ScriptableObject to define trade goods (Name, Icon, BaseValue).

## 2. World & Navigation
- [x] Create Port.cs script. Fields: string portName, Vector2 coordinates.
- [x] Implement ShipNavigation.cs. The ship should rotate towards and move to a target Port object.
- [x] Add speed clamping in ShipNavigation based on ShipStats.baseSpeed.

## 3. Trading Prototype
- [x] Create PortEconomy.cs. It should store a list of prices for different ItemData.
- [x] Implement BuyItem and SellItem methods in the Ship class to update gold and check cargoCapacity.

## 4. Basic UI
- [x] Create HUDManager.cs to display Current Gold and Cargo status on the screen.
- [x] Implement a simple UI notification when the ship arrives at a port.

## 5. Game Loop Core (Finite State Machine Implementation)

### GOAL
Introduce a centralized GameStateManager that controls high-level game flow.
All movement, UI and encounter logic must depend on this state machine.

---

### 5.1 Create GameState enum

- [x] Create GameState enum inside PirateGame.Core namespace.
- [x] Enum must contain exactly:
      - WorldMap
      - Port
      - Encounter
- [x] Do NOT add additional states.

---

### 5.2 Create GameStateManager.cs

- [x] Create GameStateManager.cs in Assets/Scripts/Core.
- [x] Namespace: PirateGame.Core.
- [x] Class must inherit from MonoBehaviour.
- [x] Add [SerializeField] private GameState currentState initialized to WorldMap.
- [x] Add public GameState CurrentState getter (read-only).
- [x] Add public method ChangeState(GameState newState).
- [x] ChangeState must:
      - Return early if state is identical.
      - Invoke OnStateExited event before changing state.
      - Update currentState.
      - Invoke OnStateEntered event after change.
- [x] Add events:
      - public event Action<GameState> OnStateEntered;
      - public event Action<GameState> OnStateExited;

---

### 5.3 Enforce Single Source of Truth

- [x] No other class is allowed to store game state.
- [x] No other class may modify state directly.
- [x] All state transitions must call GameStateManager.ChangeState().

---

### 5.4 Integrate with ShipNavigation

- [x] Add [SerializeField] GameStateManager reference to ShipNavigation.
- [x] In FixedUpdate(), block movement if:
        CurrentState != GameState.WorldMap
- [x] When blocked:
        - Set Rigidbody2D velocity to zero.
        - Do not rotate ship.
- [x] Do NOT remove existing movement logic.
- [x] Only wrap it with state check.

---

### 5.5 Port Arrival Integration

- [x] When ShipNavigation detects arrival at Port:
        - Call gameStateManager.ChangeState(GameState.Port);
- [x] Remove any direct UI calls from ShipNavigation.
- [x] ShipNavigation must not open UI directly.

---

### 5.6 Encounter Preparation Hook

- [x] Create placeholder method in EncounterManager:
        TriggerEncounter()
- [x] TriggerEncounter must:
        - Call gameStateManager.ChangeState(GameState.Encounter);
- [x] Do NOT implement combat yet.

---

### 5.7 State Debug Logging

- [x] Add Debug.Log in GameStateManager.ChangeState:
        "GameState changed from X to Y"
- [x] Logging must be wrapped with UNITY_EDITOR conditional compilation.

---

### 5.8 Validation

Implementation must compile with:
- No namespace changes.
- No modification of existing ShipStats.
- No modification of PortEconomy.
- No modification outside:
      - GameStateManager.cs
      - ShipNavigation.cs
      - EncounterManager.cs (if exists)

---

## 6. Deep Refactoring & Architecture Upgrade (Tech Debt)

- [x] **6.1 Clean up Duplicates:** Delete `Assets/Scripts/Core/ItemData.cs`. Keep only `Assets/Scripts/Economy/ItemData.cs`. Ensure `Ship.cs` and `PortEconomy.cs` use `using PirateGame.Economy;`.
- [ ] **6.2 Implement Event-Driven UI:** In `Ship.cs` or `ShipStats.cs`, add `public event Action<int> OnGoldChanged;` and `public event Action<int, int> OnCargoChanged;` (passing current and max cargo). Trigger these events whenever gold or cargo changes.
- [ ] **6.3 Refactor HUDManager:** Migrate from legacy `UnityEngine.UI.Text` to `TMPro.TextMeshProUGUI` (requires `using TMPro;`). Remove the `Update()` method entirely. Instead, subscribe to `Ship.OnGoldChanged` and `Ship.OnCargoChanged` in `Start()` / `OnEnable()` and unsubscribe in `OnDisable()`. Do the same TMPro migration for `UINotification.cs`.
- [ ] **6.4 Real Inventory System:** In `Ship.cs`, implement a real inventory using `private Dictionary<ItemData, int> cargoInventory`. 
    - `BuyItem`: Check if `currentCargoLoad < shipStats.CargoCapacity`. If true, add to dictionary, deduct gold, increase `currentCargoLoad`, trigger events.
    - `SellItem`: Check if `cargoInventory.ContainsKey(item)` and amount > 0. If true, remove from dictionary, add gold, decrease `currentCargoLoad`, trigger events.
- [ ] **6.5 Fix Physics Movement:** In `ShipNavigation.cs`, stop modifying `transform.position` and `transform.rotation` inside `FixedUpdate()`. Use `rb.MovePosition()` and `rb.MoveRotation()` strictly. Calculate the new position and rotation based on `Time.fixedDeltaTime` to ensure smooth physics step evaluation.
- [ ] **6.6 Optimize Economy Lookups:** In `PortEconomy.cs`, keep the `List<ItemPrice>` for Inspector serialization, but build a `private Dictionary<ItemData, int> runtimePrices` in the `Awake()` method. Refactor `GetItemPrice(ItemData item)` to use `runtimePrices.TryGetValue` for O(1) lookups instead of the linear `foreach` loop.

---

### ACCEPTANCE CRITERIA

The following must be true:

1. Ship moves only in WorldMap state.
2. Upon reaching port → state becomes Port.
3. In Port state → ship cannot move.
4. No NullReferenceExceptions.
5. Code compiles without warnings.

