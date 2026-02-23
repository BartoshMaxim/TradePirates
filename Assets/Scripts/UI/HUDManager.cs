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
        
        private void OnEnable()
        {
            // Subscribe to events
            if (ship != null)
            {
                ship.OnGoldChanged += UpdateGoldDisplay;
                ship.OnCargoChanged += UpdateCargoDisplay;
                
                // Initialize display with current values
                UpdateGoldDisplay(ship.GetGold());
                // Cargo display will be updated when the event fires
            }
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            if (ship != null)
            {
                ship.OnGoldChanged -= UpdateGoldDisplay;
                ship.OnCargoChanged -= UpdateCargoDisplay;
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
            if (cargoText != null)
            {
                cargoText.text = $"Cargo: {currentCargo}/{maxCargo}";
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
            }
            
            ship = targetShip;
            
            // Subscribe to new ship
            if (ship != null)
            {
                ship.OnGoldChanged += UpdateGoldDisplay;
                ship.OnCargoChanged += UpdateCargoDisplay;
                
                // Update display with new ship's values
                UpdateGoldDisplay(ship.GetGold());
                // Cargo display will be updated when the event fires
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
