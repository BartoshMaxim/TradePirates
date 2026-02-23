using UnityEngine;
using PirateGame.Economy;
using System;

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
            
            // Check cargo capacity (assuming 1 unit per item)
            // Note: This is a simple implementation. You might want a cargo inventory system
            if (shipStats.CargoCapacity <= 0)
            {
                Debug.Log($"No cargo capacity available to buy {item.ItemName}");
                return false;
            }
            
            // Deduct gold
            shipStats.Gold -= price;
            
            // Reduce cargo capacity (simple implementation)
            shipStats.CargoCapacity -= 1;
            
            Debug.Log($"Bought {item.ItemName} for {price} gold. Remaining gold: {shipStats.Gold}, Cargo capacity: {shipStats.CargoCapacity}");
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
            
            int price = portEconomy.GetItemPrice(item);
            
            // Check if ship has cargo to sell (simple implementation)
            // In a real system, you'd check if the item is actually in the cargo hold
            // For now, we'll assume the ship always has items to sell if there's capacity used
            // This is a placeholder implementation
            if (shipStats.CargoCapacity >= 100) // Assuming 100 is max capacity
            {
                Debug.Log($"No cargo capacity used to sell items");
                return false;
            }
            
            // Add gold
            shipStats.Gold += price;
            
            // Increase cargo capacity (freeing up space)
            shipStats.CargoCapacity += 1;
            
            Debug.Log($"Sold {item.ItemName} for {price} gold. Total gold: {shipStats.Gold}, Cargo capacity: {shipStats.CargoCapacity}");
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
    }
}
