using UnityEngine;

namespace Player
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform camPosGarage;
        public Transform camPosStorage;

        public float cameraFlySpeed;

        public enum Room
        {
            Garage,
            Storage
        }

        public Room room;

        private void Awake()
        {
            transform.position = camPosGarage.position;
        }

        private void Update()
        {
            HandleMovementInput();
            ExecuteMovement();
        }

        private void HandleMovementInput()
        {
            float direction = Input.GetAxisRaw("Horizontal");

            room = direction switch
            {
                < 0 => Room.Garage,
                > 0 => Room.Storage,
                _ => room
            };
        }

        private void ExecuteMovement()
        {
            if (room == Room.Storage)
                MoveToRoom(camPosStorage);
            else
                MoveToRoom(camPosGarage);
        }

        private void MoveToRoom(Transform newRoomTransformToMoveTo)
        {
            transform.position = Vector3.Lerp(transform.position, newRoomTransformToMoveTo.position, cameraFlySpeed * Time.deltaTime);
            
        }
    }
}
