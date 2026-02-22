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
