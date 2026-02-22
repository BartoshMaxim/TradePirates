using UnityEngine;

namespace PirateGame.Navigation
{
    public class Port : MonoBehaviour
    {
        [SerializeField] private string portName;
        [SerializeField] private Vector2 coordinates;

        public string PortName => portName;
        public Vector2 Coordinates => coordinates;
    }
}
