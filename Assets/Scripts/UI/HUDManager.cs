using UnityEngine;
using TMPro;
using PirateGame.Core;

namespace PirateGame.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Ship ship;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI cargoText;
        
        private Inventory shipInventory;
        
        private void OnEnable()
        {
            // Subscribe to events
            if (ship != null)
            {
                ship.OnGoldChanged += UpdateGoldDisplay;
                ship.OnCargoChanged += UpdateCargoDisplay;
                
                // Get the inventory from ship stats
                if (ship.GetComponent<ShipStats>()?.Inventory != null)
                {
                    shipInventory = ship.GetComponent<ShipStats>().Inventory;
                    shipInventory.OnInventoryChanged += UpdateInventoryDisplay;
                }
                
                // Initialize display with current values
                UpdateGoldDisplay(ship.GetGold());
                UpdateInventoryDisplay();
            }
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            if (ship != null)
            {
                ship.OnGoldChanged -= UpdateGoldDisplay;
                ship.OnCargoChanged -= UpdateCargoDisplay;
                
                if (shipInventory != null)
                {
                    shipInventory.OnInventoryChanged -= UpdateInventoryDisplay;
                }
            }
        }
        
        private void Start()
        {
            // Try to find Ship component if not assigned
            if (ship == null)
            {
                ship = FindObjectOfType<Ship>();
            }
            
            if (ship == null)
            {
                Debug.LogError("HUDManager: No Ship component found in the scene!");
            }
            
            // Check if UI Text components are assigned
            if (goldText == null || cargoText == null)
            {
                Debug.LogError("HUDManager: GoldText or CargoText not assigned!");
            }
        }
        
        private void UpdateGoldDisplay(int gold)
        {
            if (goldText != null)
            {
                goldText.text = $"Gold: {gold}";
            }
        }
        
        private void UpdateCargoDisplay(int currentCargo, int maxCargo)
        {
            // This is kept for backward compatibility
            // But we'll use UpdateInventoryDisplay instead
        }
        
        private void UpdateInventoryDisplay()
        {
            if (cargoText != null && shipInventory != null)
            {
                float currentWeight = shipInventory.GetTotalWeight();
                // For now, display weight. In the future, we might want to display slots
                cargoText.text = $"Weight: {currentWeight:F1}/{shipInventory.MaxWeight:F1}";
            }
            else if (cargoText != null)
            {
                // Fallback to old display if no inventory
                cargoText.text = $"Cargo: 0/0";
            }
        }
        
        /// <summary>
        /// Set the ship reference for the HUD manager
        /// </summary>
        /// <param name="targetShip">The ship to track</param>
        public void SetShip(Ship targetShip)
        {
            // Unsubscribe from old ship if exists
            if (ship != null)
            {
                ship.OnGoldChanged -= UpdateGoldDisplay;
                ship.OnCargoChanged -= UpdateCargoDisplay;
                
                if (shipInventory != null)
                {
                    shipInventory.OnInventoryChanged -= UpdateInventoryDisplay;
                }
            }
            
            ship = targetShip;
            
            // Subscribe to new ship
            if (ship != null)
            {
                ship.OnGoldChanged += UpdateGoldDisplay;
                ship.OnCargoChanged += UpdateCargoDisplay;
                
                // Get the inventory from ship stats
                if (ship.GetComponent<ShipStats>()?.Inventory != null)
                {
                    shipInventory = ship.GetComponent<ShipStats>().Inventory;
                    shipInventory.OnInventoryChanged += UpdateInventoryDisplay;
                }
                
                // Update display with new ship's values
                UpdateGoldDisplay(ship.GetGold());
                UpdateInventoryDisplay();
            }
        }
        
        /// <summary>
        /// Set the UI Text component for displaying gold
        /// </summary>
        /// <param name="textComponent">The Text component for gold display</param>
        public void SetGoldText(TextMeshProUGUI textComponent)
        {
            goldText = textComponent;
        }
        
        /// <summary>
        /// Set the UI Text component for displaying cargo status
        /// </summary>
        /// <param name="textComponent">The Text component for cargo display</param>
        public void SetCargoText(TextMeshProUGUI textComponent)
        {
            cargoText = textComponent;
        }
    }
}
