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

- [ ] Create GameStateManager.cs in Assets/Scripts/Core.
- [ ] Namespace: PirateGame.Core.
- [ ] Class must inherit from MonoBehaviour.
- [ ] Add [SerializeField] private GameState currentState initialized to WorldMap.
- [ ] Add public GameState CurrentState getter (read-only).
- [ ] Add public method ChangeState(GameState newState).
- [ ] ChangeState must:
      - Return early if state is identical.
      - Invoke OnStateExited event before changing state.
      - Update currentState.
      - Invoke OnStateEntered event after change.
- [ ] Add events:
      - public event Action<GameState> OnStateEntered;
      - public event Action<GameState> OnStateExited;

---

### 5.3 Enforce Single Source of Truth

- [ ] No other class is allowed to store game state.
- [ ] No other class may modify state directly.
- [ ] All state transitions must call GameStateManager.ChangeState().

---

### 5.4 Integrate with ShipNavigation

- [ ] Add [SerializeField] GameStateManager reference to ShipNavigation.
- [ ] In FixedUpdate(), block movement if:
        CurrentState != GameState.WorldMap
- [ ] When blocked:
        - Set Rigidbody2D velocity to zero.
        - Do not rotate ship.
- [ ] Do NOT remove existing movement logic.
- [ ] Only wrap it with state check.

---

### 5.5 Port Arrival Integration

- [ ] When ShipNavigation detects arrival at Port:
        - Call gameStateManager.ChangeState(GameState.Port);
- [ ] Remove any direct UI calls from ShipNavigation.
- [ ] ShipNavigation must not open UI directly.

---

### 5.6 Encounter Preparation Hook

- [ ] Create placeholder method in EncounterManager:
        TriggerEncounter()
- [ ] TriggerEncounter must:
        - Call gameStateManager.ChangeState(GameState.Encounter);
- [ ] Do NOT implement combat yet.

---

### 5.7 State Debug Logging

- [ ] Add Debug.Log in GameStateManager.ChangeState:
        "GameState changed from X to Y"
- [ ] Logging must be wrapped with UNITY_EDITOR conditional compilation.

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

### ACCEPTANCE CRITERIA

The following must be true:

1. Ship moves only in WorldMap state.
2. Upon reaching port → state becomes Port.
3. In Port state → ship cannot move.
4. No NullReferenceExceptions.
5. Code compiles without warnings.

