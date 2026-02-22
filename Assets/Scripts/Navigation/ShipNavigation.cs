using UnityEngine;
using PirateGame.UI;

namespace PirateGame.Navigation
{
    public class ShipNavigation : MonoBehaviour
    {
        [SerializeField] private Port targetPort;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float arrivalThreshold = 0.5f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private UINotification uiNotification;

        private ShipStats shipStats;
        private bool isMoving = false;

        void Start()
        {
            shipStats = GetComponent<ShipStats>();
            if (shipStats == null)
            {
                Debug.LogError("ShipStats component not found on the same GameObject!");
            }
        }

        void Update()
        {
            if (targetPort == null || !isMoving) return;

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
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = targetPort.Coordinates;

            float distance = Vector2.Distance(currentPosition, targetPosition);
            
            if (distance <= arrivalThreshold)
            {
                isMoving = false;
                Debug.Log($"Arrived at {targetPort.PortName}");
                
                // Trigger UI notification
                if (uiNotification != null)
                {
                    uiNotification.ShowNotification($"Arrived at {targetPort.PortName}");
                }
                return;
            }

            float speed = shipStats != null ? shipStats.BaseSpeed : 1f;
            speed = Mathf.Clamp(speed, 0f, maxSpeed);
            
            Vector2 direction = (targetPosition - currentPosition).normalized;
            Vector2 movement = direction * speed * Time.deltaTime;
            
            transform.position += new Vector3(movement.x, movement.y, 0);
        }

        private void RotateTowardsTarget()
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = targetPort.Coordinates;
            
            Vector2 direction = targetPosition - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
