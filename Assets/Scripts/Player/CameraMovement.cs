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
            if (Input.GetKeyDown(KeyCode.A)) room = Room.Garage;
            if (Input.GetKeyDown(KeyCode.D)) room = Room.Storage;
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
