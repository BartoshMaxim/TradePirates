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
                    // Calculate current cargo load: max - available capacity
                    int currentLoad = maxCargoCapacity - cargoCapacity;
                    OnCargoChanged?.Invoke(currentLoad, maxCargoCapacity);
                }
            }
        }

        public Inventory Inventory => inventory;

        // Helper method to get max cargo capacity (for UI)
        public int GetMaxCargoCapacity()
        {
            return maxCargoCapacity;
        }
    }
}
