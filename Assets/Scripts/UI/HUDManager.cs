using UnityEngine;
using UnityEngine.UI;

namespace PirateGame.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Core.Ship ship;
        [SerializeField] private Text goldText;
        [SerializeField] private Text cargoText;
        
        private void Start()
        {
            // Try to find Ship component if not assigned
            if (ship == null)
            {
                ship = FindObjectOfType<Core.Ship>();
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
        
        private void Update()
        {
            if (ship == null || goldText == null || cargoText == null) return;
            
            // Update gold display
            int currentGold = ship.GetGold();
            goldText.text = $"Gold: {currentGold}";
            
            // Update cargo display
            int currentCargo = ship.GetCargoCapacity();
            cargoText.text = $"Cargo: {currentCargo}";
        }
        
        /// <summary>
        /// Set the ship reference for the HUD manager
        /// </summary>
        /// <param name="targetShip">The ship to track</param>
        public void SetShip(Core.Ship targetShip)
        {
            ship = targetShip;
        }
        
        /// <summary>
        /// Set the UI Text component for displaying gold
        /// </summary>
        /// <param name="textComponent">The Text component for gold display</param>
        public void SetGoldText(Text textComponent)
        {
            goldText = textComponent;
        }
        
        /// <summary>
        /// Set the UI Text component for displaying cargo status
        /// </summary>
        /// <param name="textComponent">The Text component for cargo display</param>
        public void SetCargoText(Text textComponent)
        {
            cargoText = textComponent;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using PirateGame.Core;

namespace PirateGame.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Ship ship;
        [SerializeField] private Text goldText;
        [SerializeField] private Text cargoText;
        
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
        
        private void Update()
        {
            if (ship == null || goldText == null || cargoText == null) return;
            
            // Update gold display
            int currentGold = ship.GetGold();
            goldText.text = $"Gold: {currentGold}";
            
            // Update cargo display
            int currentCargo = ship.GetCargoCapacity();
            cargoText.text = $"Cargo: {currentCargo}";
        }
        
        /// <summary>
        /// Set the ship reference for the HUD manager
        /// </summary>
        /// <param name="targetShip">The ship to track</param>
        public void SetShip(Ship targetShip)
        {
            ship = targetShip;
        }
        
        /// <summary>
        /// Set the UI Text component for displaying gold
        /// </summary>
        /// <param name="textComponent">The Text component for gold display</param>
        public void SetGoldText(Text textComponent)
        {
            goldText = textComponent;
        }
        
        /// <summary>
        /// Set the UI Text component for displaying cargo status
        /// </summary>
        /// <param name="textComponent">The Text component for cargo display</param>
        public void SetCargoText(Text textComponent)
        {
            cargoText = textComponent;
        }
    }
}
