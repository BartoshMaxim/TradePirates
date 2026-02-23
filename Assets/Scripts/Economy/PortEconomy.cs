using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Economy
{
    public class PortEconomy : MonoBehaviour
    {
        [System.Serializable]
        public struct ItemPrice
        {
            public ItemData item;
            public int price;
        }

        [SerializeField] private List<ItemPrice> itemPrices = new List<ItemPrice>();
        private Dictionary<ItemData, int> runtimePrices;

        private void Awake()
        {
            // Initialize the runtime dictionary from the serialized list
            runtimePrices = new Dictionary<ItemData, int>();
            foreach (ItemPrice itemPrice in itemPrices)
            {
                if (itemPrice.item != null)
                {
                    runtimePrices[itemPrice.item] = itemPrice.price;
                }
            }
        }

        public int GetItemPrice(ItemData item)
        {
            if (item == null) return 0;
            
            // Use dictionary for O(1) lookup
            if (runtimePrices.TryGetValue(item, out int price))
            {
                return price;
            }
            return 0;
        }

        public void SetItemPrice(ItemData item, int price)
        {
            if (item == null) return;
            
            // Update or add to dictionary
            runtimePrices[item] = price;
            
            // Also update the serialized list for inspector visibility
            for (int i = 0; i < itemPrices.Count; i++)
            {
                if (itemPrices[i].item == item)
                {
                    ItemPrice updated = itemPrices[i];
                    updated.price = price;
                    itemPrices[i] = updated;
                    return;
                }
            }
            itemPrices.Add(new ItemPrice { item = item, price = price });
        }

        public List<ItemPrice> GetAllItemPrices()
        {
            return new List<ItemPrice>(itemPrices);
        }

        public void InitializeWithDefaultPrices(List<ItemData> availableItems)
        {
            // Clear existing data
            itemPrices.Clear();
            if (runtimePrices == null)
            {
                runtimePrices = new Dictionary<ItemData, int>();
            }
            else
            {
                runtimePrices.Clear();
            }
            
            foreach (ItemData item in availableItems)
            {
                if (item == null) continue;
                    
                int price = Mathf.RoundToInt(item.BaseValue * Random.Range(0.8f, 1.2f));
                itemPrices.Add(new ItemPrice { item = item, price = price });
                runtimePrices[item] = price;
            }
        }
    }
}
