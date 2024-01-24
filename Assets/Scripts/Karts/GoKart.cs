using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TMPro;
using Task;
using Random = UnityEngine.Random;

namespace Karts
{
    public class GoKart : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("UI")]
        [Space(5)]
        public AudioClip drivingInSFX;
        public AudioClip drivingOutSFX;
        
        [Space(20)]
        [Header("Car Component Lists")]
        [Space(5)]
        
        public List<Transform> distanceSlots;

        public CarComponent[] carComponents;

        public List<CarComponent> brokenParts;
        public List<CarComponent> damagedParts;
        public List<CarComponent> intactParts;

        [Space(20)]
        [Header("UI")]
        [Space(5)]
        
        public TextMeshProUGUI carComponentsUI;

        [Space(20)]
        [Header("General Settings")]
        [Space(5)]
        
        public float speed = 5f;

        public enum Status
        {
            DrivingIn,
            GetRepaired,
            DrivingOut
        }

        public Status goKartStatus;
        
        [Space(20)]
        [Header("Tutorial")]
        [Space(5)]
        
        public bool isTutorialGoKart;
        
        // Private Fields
        private GameManager gameManager;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = drivingInSFX;
        }

        private void Start()
        {
            if (!isTutorialGoKart)
                RollCarComponentsStatus();
            
            ListInOrderOfStatus();
            AddCarComponentsToTaskManager();

            goKartStatus = Status.DrivingIn;
            gameManager = TaskManager.Instance.gameManager;
            
            // Sound
            audioSource.Play();
        }

        private void Update()
        {
            switch (goKartStatus)
            {
                case Status.DrivingIn:
                    DriveToDestination(Vector3.zero);
                    break;

                case Status.GetRepaired:
                    // Nothing.
                    break;
                
                case Status.DrivingOut:
                    speed = 0.8f;
                    DriveToDestination(gameManager.goKartTargetDestination.position);
                    break;
            }
        }

        private void AddCarComponentsToTaskManager()
        {
            foreach (CarComponent brokenPart in brokenParts)
            {
                TaskManager.Instance.AddBrokenPart(brokenPart);
            }

            foreach (CarComponent damagedPart in damagedParts)
            {
                TaskManager.Instance.AddDamagedPart(damagedPart);
            }
        }

        private void RollCarComponentsStatus()
        {
            foreach (CarComponent carComponent in carComponents)
            {
                int newStatus = Random.Range(0, 3);

                carComponent.status = newStatus switch
                {
                    0 => CarComponent.Status.Broken,
                    1 => CarComponent.Status.Damaged,
                    2 => CarComponent.Status.Intact,
                    _ => carComponent.status
                };
            }
        }

        private void ListInOrderOfStatus()
        {
            foreach (CarComponent part in carComponents)
            {
                switch (part.status)
                {
                    case CarComponent.Status.Broken:
                        brokenParts.Add(part);
                        break;

                    case CarComponent.Status.Damaged:
                        damagedParts.Add(part);
                        break;

                    case CarComponent.Status.Intact:
                        intactParts.Add(part);
                        break;

                    default:
                        intactParts.Add(part);
                        break;
                }
            }
        }

        public bool IsUnitInRange(NavMeshAgent unitAgent)
        {
            foreach (Transform distanceSlot in distanceSlots)
            {
                if (Vector3.Distance(new Vector3(unitAgent.transform.position.x, distanceSlot.position.y, unitAgent.transform.position.z), distanceSlot.position) <= unitAgent.stoppingDistance * 1.8)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckForDoubledCarComponents(CarComponent equippedCarComponent)
        {
            foreach (CarComponent carComponent in carComponents)
            {
                if (carComponent is null) continue;
                
                if (carComponent.carPartType == equippedCarComponent.carPartType)
                {
                    return true;
                }
            }
            return false;
        }
        
        public int GetFreeCarComponentSlotIndex()
        {
            for (var i = 0; i < carComponents.Length; i++)
            {
                if (carComponents[i] is null) return i;
            }

            return -1;
        }

        private void DriveToDestination(Vector3 newDestination)
        {
            transform.position = Vector3.Lerp(transform.position, newDestination, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, newDestination) <= 0.5f)   // Reset status to GetRepaired (Standing) when GoKart arrives at Destination.
                if (goKartStatus == Status.DrivingIn)
                    goKartStatus = Status.GetRepaired;
                else
                    gameManager.NextGoKart();
        }

        public void PlayDrivingOutSFX()
        {
            audioSource.clip = drivingOutSFX;
            audioSource.Play();
        }
    }
}
