using UnityEngine;
using UnityEngine.AI;
using System;
using Karts;
using Task;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SelectableUnit : MonoBehaviour
    {
        public SpriteRenderer selectionSprite;
        
        private NavMeshAgent agent;
        private ToolStation lastToolStationToReach;
        private ComponentStation lastComponentStationToReach;
        private GoKart currentGoKart;
        
        public Transform toolSlot;
        public Transform componentSlot;
        private Tool equippedTool;
        private CarComponent equippedCarComponent;

        private Vector3 currentNewDestination;

        public enum States
        {
            Idle,
            MoveToDestination,
            GetTool,
            GetCarComponent,
            RepairKart,
            RemoveCarComponent,
            AddCarComponent
        }
        
        public States currentState;
        
        private void Awake()
        {
            SelectionManager.Instance.AvailableUnits.Add(this);
            agent = GetComponent<NavMeshAgent>();
            currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            toolSlot = transform.Find("Tool Slot");
            componentSlot = transform.Find("Component Slot");
            
            currentState = States.Idle;
        }
        
        public void OnSelected()
        {
            selectionSprite.gameObject.SetActive(true);
        }

        public void OnDeselected()
        {
            selectionSprite.gameObject.SetActive(false);
        }

        public void MoveToDestination(Vector3 newDestination)
        {
            currentState = States.MoveToDestination;
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(transform.position, currentNewDestination);
        }

        public void GetTool(Vector3 newDestination, Transform toolToReach)
        {
            currentState = States.GetTool;
            lastToolStationToReach = toolToReach.GetComponent<ToolStation>();
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
        }

        public void GetCarComponent(Vector3 newDestination, Transform componentToReach)
        {
            currentState = States.GetCarComponent;
            lastComponentStationToReach = componentToReach.GetComponent<ComponentStation>();
            agent.SetDestination(newDestination);
        }

        public void RepairKart(Vector3 newDestination)
        {
            currentState = States.RepairKart;
            agent.SetDestination(newDestination);
        }

        public void RemoveCarComponent(Vector3 newDestination)
        {
            currentState = States.RemoveCarComponent;
            agent.SetDestination(newDestination);
        }

        public void AddCarComponent(Vector3 newDestination)
        {
            currentState = States.AddCarComponent;
            agent.SetDestination(newDestination);
        }

        private void Update()
        {
            float distanceToDestination = Vector3.Distance(transform.position, currentNewDestination);
            
            switch (currentState)
            {
                case States.Idle:
                    break;
                
                case States.MoveToDestination:
                    if (distanceToDestination <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        currentState = States.Idle;
                    }
                    Debug.Log(distanceToDestination);

                    break;
                
                case States.GetTool:
                    if (distanceToDestination <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        Debug.Log("Get Tool");
                        UPDATE_GrabTool();
                        currentState = States.Idle;
                        Debug.Log("A: " + transform.position +
                                  "   B: " + agent.pathEndPosition +
                                  "    Dist.: " + Vector3.Distance(agent.destination, agent.pathEndPosition) +
                                  "    C: " +agent.remainingDistance);
                    }
                    break;
                
                case States.GetCarComponent:
                    if (agent.remainingDistance <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        UPDATE_GetCarComponent();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.RepairKart:
                    if (agent.remainingDistance <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        UPDATE_RepairCarComponents();
                        currentState = States.Idle;
                    }
                    break;

                case States.RemoveCarComponent:
                    if (agent.remainingDistance <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        UPDATE_RemoveCarComponent();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.AddCarComponent :
                    if (agent.remainingDistance <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        UPDATE_AddCarComponent();
                        currentState = States.Idle;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private bool CheckIfUnitReachedDestination()
        {
            return agent.remainingDistance <= agent.stoppingDistance;
        }

        private void UPDATE_GrabTool()
        {
            // Check if Unit already has a Tool in hand.
            if (toolSlot.childCount != 0)
            {
                // Check if equipped Tool is from this ToolStation.
                if (toolSlot.GetChild(0).GetComponent<Tool>().toolType ==
                    lastToolStationToReach.toolPrefab.GetComponent<Tool>().toolType)
                {
                    Destroy(toolSlot.GetChild(0).gameObject);
                    equippedTool = null;
                }
                return;
            }
            
            // Check if Unit already has a CarComponent in hand.
            if (componentSlot.childCount != 0) return;
            
            // Add Tool to Unit's Tool Slot (transform)
            Instantiate(lastToolStationToReach.toolPrefab, toolSlot);
            equippedTool = lastToolStationToReach.toolPrefab.GetComponent<Tool>();
        }

        private void UPDATE_GetCarComponent()               // TODO: TEST THIS MECHANIC
        {
            Debug.Log("UPDATE_GetCarComponent");
            // Check if Unit already has a CarComponent in hand
            if (componentSlot.childCount != 0)
            {
                // Check if equipped CarComponent is from this ComponentStation
                if (componentSlot.GetChild(0).GetComponent<CarComponent>().carPartType ==
                    lastComponentStationToReach.carComponentPrefab.GetComponent<CarComponent>().carPartType)
                {
                    Destroy(componentSlot.GetChild(0).gameObject);
                    equippedCarComponent = null;
                }
                return;
            }
            
            // Check if Unit already has a Tool in hand.
            if (toolSlot.childCount != 0) return;
            
            // Add CarComponent to Unit's Component Slot (transform)
            Instantiate(lastComponentStationToReach.carComponentPrefab, componentSlot);
            equippedCarComponent = lastComponentStationToReach.carComponentPrefab.GetComponent<CarComponent>();
        }

        private void UPDATE_RepairCarComponents()
        {
            if (toolSlot.childCount == 0) return;                           // Checks if unit has tool
            
            if (TaskManager.Instance.damagedParts.Count == 0) return;       // Checks if car has damaged parts
            
            CarComponent partToRepair = null;                               // Creates place for partToRepair

            // Goes through all damaged parts.
            // The last damaged part which toolToGetRepaired matches the equipped tool gets saved.
            foreach (CarComponent damagedPart in TaskManager.Instance.damagedParts)
                if (damagedPart.toolToRepair.name == equippedTool.name)
                    partToRepair = damagedPart;
            
            // If there is no part which could get repaired witch the currently equipped tool -> return.
            if (partToRepair == null) return;
            
            // Removes the damagedPart from Damaged-List and Adds it to Intact-List.
            currentGoKart.damagedParts.Remove(partToRepair);
            currentGoKart.intactParts.Add(partToRepair);
            TaskManager.Instance.RemoveDamagedPart(partToRepair);
        }

        private void UPDATE_RemoveCarComponent()
        {
            Debug.Log("UPDATE_RemoveCarComponent");
            // TODO: QuickTime-Event

            // TODO: Remove Car Component from List<CarComponent> brokenComponents.
            
            // TODO: Add Mesh-Instance per Instantiate at units ToolSlot.
            
            // TODO: Unit should be able to through it away in a "trash-bin".
        }

        private void UPDATE_AddCarComponent()
        {
            Debug.Log("UPDATE_AddCarComponent");
            // TODO: QuickTime-Event
            
            // TODO: Add Car Component to List<CarComponent> intactComponents.
            
            // TODO: Remove instantiated CarComponent.
        }
    }
}
