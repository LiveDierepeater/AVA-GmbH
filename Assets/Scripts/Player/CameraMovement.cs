using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera cam;
        
        public Transform camPosGarage;
        public Transform camPosStorage;

        public float cameraFlySpeed;
        public float scrollAmount = 1f;
        public float scrollSpeed = 10f;

        public float minFOV = 50f;
        public float maxFOV = 60f;

        private float newFOV;

        public enum Room
        {
            Garage,
            Storage
        }

        public Room room;

        private void Awake()
        {
            transform.position = camPosGarage.position;
            cam = GetComponent<Camera>();
            newFOV = maxFOV;
        }

        private void Update()
        {
            HandleMovementInput();
            ExecuteMovement();
            ExecuteZooming();
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

        private void ExecuteZooming()
        {
            if (Input.mouseScrollDelta.y > 0.05f)
                newFOV -= scrollAmount;
            if (Input.mouseScrollDelta.y < -0.05f)
                newFOV += scrollAmount;

            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newFOV, Time.deltaTime * scrollSpeed);
        }
    }
}
