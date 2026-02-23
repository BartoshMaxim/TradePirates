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

### ACCEPTANCE CRITERIA

The following must be true:

1. Ship moves only in WorldMap state.
2. Upon reaching port → state becomes Port.
3. In Port state → ship cannot move.
4. No NullReferenceExceptions.
5. Code compiles without warnings.
---

## 6. Deep Refactoring & Architecture Upgrade (Tech Debt)

- [x] **6.1 Clean up Duplicates:** Delete `Assets/Scripts/Core/ItemData.cs`. Keep only `Assets/Scripts/Economy/ItemData.cs`. Ensure `Ship.cs` and `PortEconomy.cs` use `using PirateGame.Economy;`.
- [x] **6.2 Implement Event-Driven UI:** In `Ship.cs` or `ShipStats.cs`, add `public event Action<int> OnGoldChanged;` and `public event Action<int, int> OnCargoChanged;` (passing current and max cargo). Trigger these events whenever gold or cargo changes.
- [x] **6.3 Refactor HUDManager:** Migrate from legacy `UnityEngine.UI.Text` to `TMPro.TextMeshProUGUI` (requires `using TMPro;`). Remove the `Update()` method entirely. Instead, subscribe to `Ship.OnGoldChanged` and `Ship.OnCargoChanged` in `Start()` / `OnEnable()` and unsubscribe in `OnDisable()`. Do the same TMPro migration for `UINotification.cs`.
- [x] **6.4 Real Inventory System:** In `Ship.cs`, implement a real inventory using `private Dictionary<ItemData, int> cargoInventory`. 
    - `BuyItem`: Check if `currentCargoLoad < shipStats.CargoCapacity`. If true, add to dictionary, deduct gold, increase `currentCargoLoad`, trigger events.
    - `SellItem`: Check if `cargoInventory.ContainsKey(item)` and amount > 0. If true, remove from dictionary, add gold, decrease `currentCargoLoad`, trigger events.
- [x] **6.5 Fix Physics Movement:** In `ShipNavigation.cs`, stop modifying `transform.position` and `transform.rotation` inside `FixedUpdate()`. Use `rb.MovePosition()` and `rb.MoveRotation()` strictly. Calculate the new position and rotation based on `Time.fixedDeltaTime` to ensure smooth physics step evaluation.
- [ ] **6.6 Optimize Economy Lookups:** In `PortEconomy.cs`, keep the `List<ItemPrice>` for Inspector serialization, but build a `private Dictionary<ItemData, int> runtimePrices` in the `Awake()` method. Refactor `GetItemPrice(ItemData item)` to use `runtimePrices.TryGetValue` for O(1) lookups instead of the linear `foreach` loop.

---

## 7. Inventory System Refactor
GOAL: Replace primitive cargo int with a structured inventory supporting stacking, max slots, and weight capacity.

- [x] 7.1 InventoryItem Data Structure: Create InventoryItem.cs in Assets/Scripts/Core.
  - Must be a [Serializable] class (or struct) and NOT inherit from MonoBehaviour.
  - Fields: public ItemData item, public int quantity.
  - Ensure namespace is PirateGame.Core.

- [ ] 7.2 Update ItemData: In ItemData.cs (Economy namespace), add:
  - [SerializeField] private int maxStack = 99;
  - [SerializeField] private float weight = 1f;
  - Add public getters for these. Do NOT rename or remove existing fields.

- [ ] 7.3 Inventory Component: Create Inventory.cs in Assets/Scripts/Core. Inherit from MonoBehaviour.
  - Fields: [SerializeField] private List<InventoryItem> items, bool useWeightCapacity, float maxWeight, int maxSlots.
  - Events: public event Action OnInventoryChanged.
  - Methods to implement: bool AddItem(ItemData, int), bool RemoveItem(ItemData, int), bool CanAdd(ItemData, int), int GetQuantity(ItemData), float GetTotalWeight().
  - Crucial Stacking Logic: In AddItem, first try to add to an existing InventoryItem of the same ItemData without exceeding maxStack. Only create a new InventoryItem if existing stacks are full and items.Count < maxSlots.

- [ ] 7.4 Ship & ShipStats Integration: - In ShipStats.cs, add [SerializeField] private Inventory inventory; and a public getter. 
  - Deprecate or carefully replace the old currentCargo int logic. Do NOT remove other unrelated public members.
  - In Ship.cs, refactor the BuyItem and SellItem methods:
    - Buy: First check shipStats.Gold >= price AND Inventory.CanAdd(item, amount). Only if BOTH are true: deduct gold, call Inventory.AddItem, and trigger UI events.
    - Sell: First check Inventory.GetQuantity(item) >= amount. If true: call Inventory.RemoveItem, add gold, trigger UI events.

- [ ] 7.5 HUD Update: In HUDManager.cs (PirateGame.UI namespace):
  - Subscribe to Inventory.OnInventoryChanged in Start or OnEnable.
  - Update the UI TextMeshPro fields to display: "Gold: [X]" and "Weight: [Current]/[Max]" (or Slots).
  
### ACCEPTANCE CRITERIA:
- Player can buy items, and they correctly stack in the Inventory list.
- Weight/slots limits are strictly respected (cannot buy if CanAdd is false).
- Selling correctly reduces the quantity and frees up slots if quantity reaches 0.
- Existing systems compile without NullReferenceExceptions.


