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

        public int GetItemPrice(ItemData item)
        {
            foreach (ItemPrice itemPrice in itemPrices)
            {
                if (itemPrice.item == item)
                {
                    return itemPrice.price;
                }
            }
            return 0;
        }

        public void SetItemPrice(ItemData item, int price)
        {
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
            foreach (ItemData item in availableItems)
            {
                int price = Mathf.RoundToInt(item.BaseValue * Random.Range(0.8f, 1.2f));
                SetItemPrice(item, price);
            }
        }
    }
}
