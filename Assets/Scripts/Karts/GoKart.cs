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
        public List<Transform> distanceSlots;

        public CarComponent[] carComponents;

        public List<CarComponent> brokenParts;
        public List<CarComponent> damagedParts;
        public List<CarComponent> intactParts;

        public bool debugCarComponents;
        public bool debugCarComponentsUI;

        public TextMeshProUGUI carComponentsUI;

        private GameManager gameManager;
        public float speed = 5f;

        public enum Status
        {
            DrivingIn,
            GetRepaired,
            DrivingOut
        }

        public Status goKartStatus;

        private void Start()
        {
            RollCarComponentsStatus();
            ListInOrderOfStatus();
            AddCarComponentsToTaskManager();

            DEBUG_CarComponents();
            DEBUG_CarComponentsUI();

            goKartStatus = Status.DrivingIn;
            gameManager = TaskManager.Instance.gameManager;
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

        private void DEBUG_CarComponents()
        {
            if (!debugCarComponents) return;

            foreach (CarComponent carComponent in brokenParts)
            {
                Debug.Log(carComponent.carPartType + ", " + carComponent.status);
            }

            foreach (CarComponent carComponent in damagedParts)
            {
                Debug.Log(carComponent.carPartType + ", " + carComponent.status);
            }

            foreach (CarComponent carComponent in intactParts)
            {
                Debug.Log(carComponent.carPartType + ", " + carComponent.status);
            }
        }

        private void DEBUG_CarComponentsUI() // Shows CarComponents in UI
        {
            if (!debugCarComponentsUI) return;

            carComponentsUI.text = "";

            foreach (CarComponent carComponent in carComponents)
            {
                if (carComponent.status != CarComponent.Status.Damaged) continue;

                carComponentsUI.text += carComponent.name + ", Tool: " + carComponent.toolToRepair.name;
                carComponentsUI.text += "\n";
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
    }
}
