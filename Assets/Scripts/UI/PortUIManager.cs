using UnityEngine;
using PirateGame.Core;
using PirateGame.Economy;
using System.Collections.Generic;

namespace PirateGame.UI
{
    public class PortUIManager : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private GameObject portUIPanel;
        [SerializeField] private Transform itemListContainer;
        [SerializeField] private GameObject itemUIPrefab;

        private PortEconomy currentPortEconomy;
        private Ship currentShip;

        private void Start()
        {
            if (gameStateManager == null)
            {
                gameStateManager = FindObjectOfType<GameStateManager>();
            }

            if (gameStateManager != null)
            {
                gameStateManager.OnStateEntered += HandleStateEntered;
                gameStateManager.OnStateExited += HandleStateExited;
            }

            // Start with UI hidden
            if (portUIPanel != null)
            {
                portUIPanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (gameStateManager != null)
            {
                gameStateManager.OnStateEntered -= HandleStateEntered;
                gameStateManager.OnStateExited -= HandleStateExited;
            }
        }

        private void HandleStateEntered(GameState newState)
        {
            if (newState == GameState.Port)
            {
                // We'll need to get the current port and ship references
                // This will be called by external code
            }
        }

        private void HandleStateExited(GameState oldState)
        {
            if (oldState == GameState.Port)
            {
                HidePortUI();
            }
        }

        /// <summary>
        /// Show the port UI with the given port economy and ship
        /// </summary>
        /// <param name="portEconomy">The port's economy system</param>
        /// <param name="ship">The player's ship</param>
        public void ShowPortUI(PortEconomy portEconomy, Ship ship)
        {
            if (portUIPanel == null || itemListContainer == null || itemUIPrefab == null)
            {
                Debug.LogError("PortUIManager: Required UI components not assigned!");
                return;
            }

            currentPortEconomy = portEconomy;
            currentShip = ship;

            portUIPanel.SetActive(true);
            UpdateItemList(portEconomy, ship);
        }

        /// <summary>
        /// Hide the port UI
        /// </summary>
        public void HidePortUI()
        {
            if (portUIPanel != null)
            {
                portUIPanel.SetActive(false);
            }
            currentPortEconomy = null;
            currentShip = null;
        }

        /// <summary>
        /// Update the item list in the UI
        /// </summary>
        /// <param name="portEconomy">The port's economy system</param>
        /// <param name="ship">The player's ship</param>
        public void UpdateItemList(PortEconomy portEconomy, Ship ship)
        {
            if (itemListContainer == null || itemUIPrefab == null)
            {
                Debug.LogError("PortUIManager: Item list components not assigned!");
                return;
            }

            // Clear existing children
            foreach (Transform child in itemListContainer)
            {
                Destroy(child.gameObject);
            }

            // Get all item prices from the port economy
            List<PortEconomy.ItemPrice> itemPrices = portEconomy.GetAllItemPrices();

            // Create UI elements for each item
            foreach (PortEconomy.ItemPrice itemPrice in itemPrices)
            {
                if (itemPrice.item == null) continue;

                GameObject itemUIObject = Instantiate(itemUIPrefab, itemListContainer);
                PortItemUI portItemUI = itemUIObject.GetComponent<PortItemUI>();

                if (portItemUI != null)
                {
                    int playerQuantity = ship.GetItemQuantity(itemPrice.item);
                    portItemUI.Setup(itemPrice.item, itemPrice.price, playerQuantity);
                }
                else
                {
                    Debug.LogWarning("PortUIManager: itemUIPrefab doesn't have PortItemUI component!");
                }
            }
        }

        /// <summary>
        /// Refresh the UI with current data
        /// </summary>
        public void RefreshUI()
        {
            if (currentPortEconomy != null && currentShip != null)
            {
                UpdateItemList(currentPortEconomy, currentShip);
            }
        }
    }
}
