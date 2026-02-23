using UnityEngine;
using PirateGame.Economy;
using System;
using System.Collections.Generic;

namespace PirateGame.Core
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private ShipStats shipStats;
        
        // Event-driven UI events exposed by Ship
        public event Action<int> OnGoldChanged;
        public event Action<int, int> OnCargoChanged;
        
        private Dictionary<ItemData, int> cargoInventory = new Dictionary<ItemData, int>();
        private int currentCargoLoad = 0;
        
        private void Start()
        {
            // Auto-get ShipStats if not assigned
            if (shipStats == null)
            {
                shipStats = GetComponent<ShipStats>();
            }
            
            if (shipStats == null)
            {
                Debug.LogError("ShipStats component not found on the same GameObject!");
            }
            else
            {
                // Forward events from ShipStats
                shipStats.OnGoldChanged += HandleGoldChanged;
                shipStats.OnCargoChanged += HandleCargoChanged;
                
                // Initialize currentCargoLoad based on starting cargo capacity
                currentCargoLoad = shipStats.GetMaxCargoCapacity() - shipStats.CargoCapacity;
            }
        }
        
        private void OnDestroy()
        {
            if (shipStats != null)
            {
                shipStats.OnGoldChanged -= HandleGoldChanged;
                shipStats.OnCargoChanged -= HandleCargoChanged;
            }
        }
        
        private void HandleGoldChanged(int gold)
        {
            OnGoldChanged?.Invoke(gold);
        }
        
        private void HandleCargoChanged(int currentCargo, int maxCargo)
        {
            OnCargoChanged?.Invoke(currentCargo, maxCargo);
        }
        
        /// <summary>
        /// Attempt to buy an item from a port economy.
        /// Checks if ship has enough gold and cargo capacity.
        /// </summary>
        /// <param name="item">The item to buy</param>
        /// <param name="portEconomy">The port's economy system</param>
        /// <returns>True if purchase was successful, false otherwise</returns>
        public bool BuyItem(ItemData item, PortEconomy portEconomy)
        {
            if (shipStats == null || item == null || portEconomy == null)
            {
                Debug.LogWarning("Cannot buy item: Missing required components");
                return false;
            }
            
            int price = portEconomy.GetItemPrice(item);
            
            // Check if ship has enough gold
            if (shipStats.Gold < price)
            {
                Debug.Log($"Not enough gold to buy {item.ItemName}. Need {price}, have {shipStats.Gold}");
                return false;
            }
            
            // Check cargo capacity
            if (currentCargoLoad >= shipStats.GetMaxCargoCapacity())
            {
                Debug.Log($"No cargo capacity available to buy {item.ItemName}");
                return false;
            }
            
            // Deduct gold
            shipStats.Gold -= price;
            
            // Add item to inventory
            if (cargoInventory.ContainsKey(item))
            {
                cargoInventory[item]++;
            }
            else
            {
                cargoInventory[item] = 1;
            }
            
            // Increase cargo load
            currentCargoLoad++;
            
            // Update cargo capacity in ShipStats (this will trigger the OnCargoChanged event)
            shipStats.CargoCapacity = shipStats.GetMaxCargoCapacity() - currentCargoLoad;
            
            Debug.Log($"Bought {item.ItemName} for {price} gold. Remaining gold: {shipStats.Gold}, Cargo load: {currentCargoLoad}/{shipStats.GetMaxCargoCapacity()}");
            return true;
        }
        
        /// <summary>
        /// Attempt to sell an item to a port economy.
        /// </summary>
        /// <param name="item">The item to sell</param>
        /// <param name="portEconomy">The port's economy system</param>
        /// <returns>True if sale was successful, false otherwise</returns>
        public bool SellItem(ItemData item, PortEconomy portEconomy)
        {
            if (shipStats == null || item == null || portEconomy == null)
            {
                Debug.LogWarning("Cannot sell item: Missing required components");
                return false;
            }
            
            // Check if item exists in inventory
            if (!cargoInventory.ContainsKey(item) || cargoInventory[item] <= 0)
            {
                Debug.Log($"No {item.ItemName} in cargo to sell");
                return false;
            }
            
            int price = portEconomy.GetItemPrice(item);
            
            // Add gold
            shipStats.Gold += price;
            
            // Remove item from inventory
            cargoInventory[item]--;
            if (cargoInventory[item] <= 0)
            {
                cargoInventory.Remove(item);
            }
            
            // Decrease cargo load
            currentCargoLoad--;
            
            // Update cargo capacity in ShipStats (this will trigger the OnCargoChanged event)
            shipStats.CargoCapacity = shipStats.GetMaxCargoCapacity() - currentCargoLoad;
            
            Debug.Log($"Sold {item.ItemName} for {price} gold. Total gold: {shipStats.Gold}, Cargo load: {currentCargoLoad}/{shipStats.GetMaxCargoCapacity()}");
            return true;
        }
        
        /// <summary>
        /// Get the ship's current gold amount
        /// </summary>
        public int GetGold()
        {
            return shipStats != null ? shipStats.Gold : 0;
        }
        
        /// <summary>
        /// Get the ship's current cargo capacity
        /// </summary>
        public int GetCargoCapacity()
        {
            return shipStats != null ? shipStats.CargoCapacity : 0;
        }
        
        /// <summary>
        /// Get the current cargo load
        /// </summary>
        public int GetCurrentCargoLoad()
        {
            return currentCargoLoad;
        }
        
        /// <summary>
        /// Get the maximum cargo capacity
        /// </summary>
        public int GetMaxCargoCapacity()
        {
            return shipStats != null ? shipStats.GetMaxCargoCapacity() : 0;
        }
        
        /// <summary>
        /// Get the cargo inventory
        /// </summary>
        public Dictionary<ItemData, int> GetCargoInventory()
        {
            return new Dictionary<ItemData, int>(cargoInventory);
        }
        
        /// <summary>
        /// Get the quantity of a specific item in cargo
        /// </summary>
        public int GetItemQuantity(ItemData item)
        {
            return cargoInventory.ContainsKey(item) ? cargoInventory[item] : 0;
        }
    }
}
