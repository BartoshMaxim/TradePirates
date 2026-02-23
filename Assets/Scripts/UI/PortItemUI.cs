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

        /// <summary>
        /// Setup the UI element with item data
        /// </summary>
        /// <param name="item">The item data</param>
        /// <param name="price">The current price at this port</param>
        /// <param name="playerQuantity">How many the player currently has</param>
        public void Setup(ItemData item, int price, int playerQuantity)
        {
            currentItem = item;
            currentPrice = price;
            currentPlayerQuantity = playerQuantity;

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
            // This will need to be connected to the Ship's BuyItem method
            // For now, we'll just log
            Debug.Log($"Buy clicked for {currentItem.ItemName} at price {currentPrice}");
        }

        private void OnSellClicked()
        {
            // This will need to be connected to the Ship's SellItem method
            // For now, we'll just log
            Debug.Log($"Sell clicked for {currentItem.ItemName} at price {currentPrice}");
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
