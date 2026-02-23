using UnityEngine;
using System;
using System.Collections.Generic;
using PirateGame.Economy;

namespace PirateGame.Core
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
        [SerializeField] private bool useWeightCapacity = false;
        [SerializeField] private float maxWeight = 100f;
        [SerializeField] private int maxSlots = 20;

        public event Action OnInventoryChanged;

        public bool AddItem(ItemData item, int amount)
        {
            if (!CanAdd(item, amount))
                return false;

            // Try to add to existing stacks first
            for (int i = 0; i < items.Count; i++)
            {
                InventoryItem existing = items[i];
                if (existing.item == item)
                {
                    int spaceInStack = item.MaxStack - existing.quantity;
                    if (spaceInStack > 0)
                    {
                        int toAdd = Mathf.Min(spaceInStack, amount);
                        existing.quantity += toAdd;
                        items[i] = existing;
                        amount -= toAdd;
                        
                        if (amount <= 0)
                        {
                            OnInventoryChanged?.Invoke();
                            return true;
                        }
                    }
                }
            }

            // If there's still amount left, create new slots
            while (amount > 0)
            {
                if (items.Count >= maxSlots)
                    return false;

                int toAdd = Mathf.Min(item.MaxStack, amount);
                items.Add(new InventoryItem(item, toAdd));
                amount -= toAdd;
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool RemoveItem(ItemData item, int amount)
        {
            if (GetQuantity(item) < amount)
                return false;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                InventoryItem existing = items[i];
                if (existing.item == item)
                {
                    int toRemove = Mathf.Min(existing.quantity, amount);
                    existing.quantity -= toRemove;
                    amount -= toRemove;

                    if (existing.quantity <= 0)
                    {
                        items.RemoveAt(i);
                    }
                    else
                    {
                        items[i] = existing;
                    }

                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool CanAdd(ItemData item, int amount)
        {
            if (useWeightCapacity)
            {
                float currentWeight = GetTotalWeight();
                float additionalWeight = item.Weight * amount;
                if (currentWeight + additionalWeight > maxWeight)
                    return false;
            }

            // Check if we have enough space in existing stacks or new slots
            int remaining = amount;
            int availableSpace = 0;

            // Space in existing stacks
            foreach (InventoryItem existing in items)
            {
                if (existing.item == item)
                {
                    availableSpace += (item.MaxStack - existing.quantity);
                }
            }

            // Space in new slots
            int newSlotsAvailable = maxSlots - items.Count;
            availableSpace += newSlotsAvailable * item.MaxStack;

            return availableSpace >= amount;
        }

        public int GetQuantity(ItemData item)
        {
            int total = 0;
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item == item)
                {
                    total += inventoryItem.quantity;
                }
            }
            return total;
        }

        public float GetTotalWeight()
        {
            float total = 0f;
            foreach (InventoryItem inventoryItem in items)
            {
                total += inventoryItem.item.Weight * inventoryItem.quantity;
            }
            return total;
        }
    }
}
