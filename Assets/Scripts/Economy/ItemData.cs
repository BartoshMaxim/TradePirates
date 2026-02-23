using UnityEngine;

namespace PirateGame.Economy
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Pirate Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField] private int baseValue;
        [SerializeField] private int maxStack = 99;
        [SerializeField] private float weight = 1f;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public int BaseValue => baseValue;
        public int MaxStack => maxStack;
        public float Weight => weight;
    }
}
