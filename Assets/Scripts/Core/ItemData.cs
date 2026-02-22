using UnityEngine;

namespace PirateGame.Core
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Pirate Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField] private int baseValue;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public int BaseValue => baseValue;
    }
}
