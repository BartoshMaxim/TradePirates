using UnityEngine;
using System;

namespace PirateGame.Core
{
    public class ShipStats : MonoBehaviour
    {
        [SerializeField] private int gold;
        [SerializeField] private float baseSpeed;
        [SerializeField] private int cargoCapacity;
        [SerializeField] private Inventory inventory;

        // Event-driven UI events
        public event Action<int> OnGoldChanged;
        public event Action<int, int> OnCargoChanged; // current cargo load, max cargo capacity

        private int maxCargoCapacity;

        private void Start()
        {
            // Initialize max cargo capacity with the starting value
            maxCargoCapacity = cargoCapacity;
            
            // Subscribe to inventory changes if inventory exists
            if (inventory != null)
            {
                inventory.OnInventoryChanged += HandleInventoryChanged;
            }
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= HandleInventoryChanged;
            }
        }

        private void HandleInventoryChanged()
        {
            // Update cargo display when inventory changes
            UpdateCargoDisplay();
        }

        private void UpdateCargoDisplay()
        {
            if (inventory != null)
            {
                // For weight-based inventory, calculate current and max
                float currentWeight = inventory.GetTotalWeight();
                float maxWeight = inventory.MaxWeight;
                
                // Convert to int for compatibility with existing event
                int currentLoad = Mathf.RoundToInt(currentWeight);
                int maxLoad = Mathf.RoundToInt(maxWeight);
                
                OnCargoChanged?.Invoke(currentLoad, maxLoad);
            }
            else
            {
                // Fallback to old system
                int currentLoad = maxCargoCapacity - cargoCapacity;
                OnCargoChanged?.Invoke(currentLoad, maxCargoCapacity);
            }
        }

        public int Gold
        {
            get => gold;
            set
            {
                if (gold != value)
                {
                    gold = value;
                    OnGoldChanged?.Invoke(gold);
                }
            }
        }

        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = value;
        }

        public int CargoCapacity
        {
            get => cargoCapacity;
            set
            {
                if (cargoCapacity != value)
                {
                    cargoCapacity = value;
                    UpdateCargoDisplay();
                }
            }
        }

        public Inventory Inventory => inventory;

        // Helper method to get max cargo capacity (for UI)
        public int GetMaxCargoCapacity()
        {
            if (inventory != null && inventory.UseWeightCapacity)
            {
                return Mathf.RoundToInt(inventory.MaxWeight);
            }
            return maxCargoCapacity;
        }
    }
}
