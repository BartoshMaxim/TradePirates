using UnityEngine;
using PirateGame.UI;
using PirateGame.Core;

namespace PirateGame.Navigation
{
    public class ShipNavigation : MonoBehaviour
    {
        [SerializeField] private Port targetPort;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float arrivalThreshold = 0.5f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private UINotification uiNotification;
        [SerializeField] private GameStateManager gameStateManager;

        private ShipStats shipStats;
        private Rigidbody2D rb;
        private bool isMoving = false;

        void Start()
        {
            shipStats = GetComponent<ShipStats>();
            if (shipStats == null)
            {
                Debug.LogError("ShipStats component not found on the same GameObject!");
            }
            
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component not found on the same GameObject!");
            }
            
            // Auto-find GameStateManager if not assigned
            if (gameStateManager == null)
            {
                gameStateManager = FindObjectOfType<GameStateManager>();
            }
        }

        void FixedUpdate()
        {
            if (targetPort == null || !isMoving) return;
            
            // Block movement if not in WorldMap state
            if (gameStateManager != null && gameStateManager.CurrentState != GameState.WorldMap)
            {
                // Don't modify isMoving so movement can resume when returning to WorldMap
                // Don't set velocity since we're using MovePosition/MoveRotation
                return;
            }

            MoveTowardsTarget();
            RotateTowardsTarget();
        }

        public void SetTargetPort(Port port)
        {
            targetPort = port;
            isMoving = true;
        }

        private void MoveTowardsTarget()
        {
            Vector2 currentPosition = rb.position;
            Vector2 targetPosition = targetPort.Coordinates;

            float distance = Vector2.Distance(currentPosition, targetPosition);
            
            if (distance <= arrivalThreshold)
            {
                isMoving = false;
                Debug.Log($"Arrived at {targetPort.PortName}");
                
                // Change game state to Port
                if (gameStateManager != null)
                {
                    gameStateManager.ChangeState(GameState.Port);
                }
                return;
            }

            float speed = shipStats != null ? shipStats.BaseSpeed : 1f;
            speed = Mathf.Clamp(speed, 0f, maxSpeed);
            
            Vector2 direction = (targetPosition - currentPosition).normalized;
            Vector2 movement = direction * speed * Time.fixedDeltaTime;
            
            // Use rb.MovePosition for physics-based movement
            if (rb != null)
            {
                rb.MovePosition(rb.position + movement);
            }
        }

        private void RotateTowardsTarget()
        {
            Vector2 currentPosition = rb.position;
            Vector2 targetPosition = targetPort.Coordinates;
            
            Vector2 direction = targetPosition - currentPosition;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Use rb.MoveRotation with angle interpolation for physics-based rotation
            if (rb != null)
            {
                float currentAngle = rb.rotation;
                float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(newAngle);
            }
        }

        public bool IsMoving()
        {
            return isMoving;
        }

        public Port GetCurrentTarget()
        {
            return targetPort;
        }

        public void SetUINotification(UINotification notification)
        {
            uiNotification = notification;
        }
    }
}
