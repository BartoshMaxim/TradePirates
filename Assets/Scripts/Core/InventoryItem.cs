using System;

namespace PirateGame.Core
{
    [Serializable]
    public class InventoryItem
    {
        public PirateGame.Economy.ItemData item;
        public int quantity;

        public InventoryItem(PirateGame.Economy.ItemData item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
using System;

namespace PirateGame.Core
{
    [Serializable]
    public class InventoryItem
    {
        public PirateGame.Economy.ItemData item;
        public int quantity;

        public InventoryItem(PirateGame.Economy.ItemData item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
