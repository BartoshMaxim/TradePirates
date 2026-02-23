using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PirateGame.Economy;

namespace PirateGame.UI
{
    public class PortItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI playerQuantityText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button sellButton;

        private ItemData currentItem;
        private int currentPrice;
        private int currentPlayerQuantity;
        private PortUIManager portUIManager;
        private Ship playerShip;
        private PortEconomy currentPortEconomy;

        /// <summary>
        /// Setup the UI element with item data
        /// </summary>
        /// <param name="item">The item data</param>
        /// <param name="price">The current price at this port</param>
        /// <param name="playerQuantity">How many the player currently has</param>
        /// <param name="uiManager">Reference to the PortUIManager</param>
        /// <param name="ship">Reference to the player's ship</param>
        /// <param name="portEconomy">Reference to the port's economy</param>
        public void Setup(ItemData item, int price, int playerQuantity, PortUIManager uiManager, Ship ship, PortEconomy portEconomy)
        {
            currentItem = item;
            currentPrice = price;
            currentPlayerQuantity = playerQuantity;
            portUIManager = uiManager;
            playerShip = ship;
            currentPortEconomy = portEconomy;

            if (itemNameText != null)
            {
                itemNameText.text = item.ItemName;
            }

            if (priceText != null)
            {
                priceText.text = $"Price: {price} gold";
            }

            if (playerQuantityText != null)
            {
                playerQuantityText.text = $"Owned: {playerQuantity}";
            }

            // Setup button listeners
            if (buyButton != null)
            {
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(OnBuyClicked);
            }

            if (sellButton != null)
            {
                sellButton.onClick.RemoveAllListeners();
                sellButton.onClick.AddListener(OnSellClicked);
            }
        }

        private void OnBuyClicked()
        {
            if (playerShip != null && currentPortEconomy != null && currentItem != null)
            {
                bool success = playerShip.BuyItem(currentItem, currentPortEconomy);
                if (success && portUIManager != null)
                {
                    portUIManager.RefreshUI();
                    // Update the player quantity display
                    UpdatePlayerQuantity(playerShip.GetItemQuantity(currentItem));
                }
            }
        }

        private void OnSellClicked()
        {
            if (playerShip != null && currentPortEconomy != null && currentItem != null)
            {
                bool success = playerShip.SellItem(currentItem, currentPortEconomy);
                if (success && portUIManager != null)
                {
                    portUIManager.RefreshUI();
                    // Update the player quantity display
                    UpdatePlayerQuantity(playerShip.GetItemQuantity(currentItem));
                }
            }
        }

        /// <summary>
        /// Update the player quantity display
        /// </summary>
        /// <param name="newQuantity">New player quantity</param>
        public void UpdatePlayerQuantity(int newQuantity)
        {
            currentPlayerQuantity = newQuantity;
            if (playerQuantityText != null)
            {
                playerQuantityText.text = $"Owned: {newQuantity}";
            }
        }

        /// <summary>
        /// Update the price display
        /// </summary>
        /// <param name="newPrice">New price</param>
        public void UpdatePrice(int newPrice)
        {
            currentPrice = newPrice;
            if (priceText != null)
            {
                priceText.text = $"Price: {newPrice} gold";
            }
        }
    }
}
