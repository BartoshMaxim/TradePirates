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
                
                // Subscribe to inventory changes if inventory exists
                if (shipStats.Inventory != null)
                {
                    shipStats.Inventory.OnInventoryChanged += HandleInventoryChanged;
                }
            }
        }
        
        private void OnDestroy()
        {
            if (shipStats != null)
            {
                shipStats.OnGoldChanged -= HandleGoldChanged;
                shipStats.OnCargoChanged -= HandleCargoChanged;
                
                if (shipStats.Inventory != null)
                {
                    shipStats.Inventory.OnInventoryChanged -= HandleInventoryChanged;
                }
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
        
        private void HandleInventoryChanged()
        {
            // Update cargo display when inventory changes
            if (shipStats != null)
            {
                // Trigger cargo changed event with current values
                int currentLoad = shipStats.GetMaxCargoCapacity() - shipStats.CargoCapacity;
                OnCargoChanged?.Invoke(currentLoad, shipStats.GetMaxCargoCapacity());
            }
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
            
            // Check if inventory can add the item
            if (shipStats.Inventory == null || !shipStats.Inventory.CanAdd(item, 1))
            {
                Debug.Log($"No space available to buy {item.ItemName}");
                return false;
            }
            
            // Deduct gold
            shipStats.Gold -= price;
            
            // Add item to inventory
            bool added = shipStats.Inventory.AddItem(item, 1);
            if (!added)
            {
                // Refund gold if addition failed
                shipStats.Gold += price;
                Debug.LogWarning($"Failed to add {item.ItemName} to inventory after payment");
                return false;
            }
            
            Debug.Log($"Bought {item.ItemName} for {price} gold. Remaining gold: {shipStats.Gold}");
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
            if (shipStats.Inventory == null || shipStats.Inventory.GetQuantity(item) <= 0)
            {
                Debug.Log($"No {item.ItemName} in cargo to sell");
                return false;
            }
            
            int price = portEconomy.GetItemPrice(item);
            
            // Remove item from inventory
            bool removed = shipStats.Inventory.RemoveItem(item, 1);
            if (!removed)
            {
                Debug.LogWarning($"Failed to remove {item.ItemName} from inventory");
                return false;
            }
            
            // Add gold
            shipStats.Gold += price;
            
            Debug.Log($"Sold {item.ItemName} for {price} gold. Total gold: {shipStats.Gold}");
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
            if (shipStats == null || shipStats.Inventory == null) return 0;
            // This needs to be updated to use inventory weight or count
            // For now, we'll use a placeholder
            return 0;
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
            // This needs to be updated to work with the new Inventory system
            // For now, return an empty dictionary
            return new Dictionary<ItemData, int>();
        }
        
        /// <summary>
        /// Get the quantity of a specific item in cargo
        /// </summary>
        public int GetItemQuantity(ItemData item)
        {
            return shipStats?.Inventory?.GetQuantity(item) ?? 0;
        }
    }
}
