using UnityEngine;

namespace PirateGame.Core
{
    public class ShipStats : MonoBehaviour
    {
        [SerializeField] private int gold;
        [SerializeField] private float baseSpeed;
        [SerializeField] private int cargoCapacity;

        public int Gold
        {
            get => gold;
            set => gold = value;
        }

        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = value;
        }

        public int CargoCapacity
        {
            get => cargoCapacity;
            set => cargoCapacity = value;
        }
    }
}
