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
            agent.SetDestination(newDestination);
        }

        public void GetTool(Vector3 newDestination, Transform toolToReach)
        {
            currentState = States.GetTool;
            lastToolStationToReach = toolToReach.GetComponent<ToolStation>();
            agent.SetDestination(newDestination);
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
            switch (currentState)
            {
                case States.Idle:
                    break;
                
                case States.MoveToDestination:
                    if (agent.remainingDistance <= agent.stoppingDistance)  // Checks if destination is reached
                        currentState = States.Idle;
                    break;
                
                case States.GetTool:
                    if (!CheckIfUnitReachedDestination()) break;
                    UPDATE_GrabTool();
                    currentState = States.Idle;
                    break;
                
                case States.GetCarComponent:
                    if (!CheckIfUnitReachedDestination()) break;
                    UPDATE_GetCarComponent();
                    currentState = States.Idle;
                    break;
                
                case States.RepairKart:
                    if (!CheckIfUnitReachedDestination()) break;
                    UPDATE_RepairCarComponents();
                    currentState = States.Idle;
                    break;

                case States.RemoveCarComponent:
                    if (!CheckIfUnitReachedDestination()) break;
                    UPDATE_RemoveCarComponent();
                    currentState = States.Idle;
                    break;
                
                case States.AddCarComponent :
                    if (!CheckIfUnitReachedDestination()) break;
                    UPDATE_AddCarComponent();
                    currentState = States.Idle;
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
            // Check if Unit already has a tool in hand.
            if (toolSlot.transform.childCount != 0)
            {
                // Check if equipped Tool is from this ToolStation.
                if (toolSlot.transform.GetChild(0).GetComponent<Tool>().toolType ==
                    lastToolStationToReach.toolPrefab.GetComponent<Tool>().toolType)
                {
                    Destroy(toolSlot.transform.GetChild(0).gameObject);
                    equippedTool = null;
                }
                return;
            }
            
            // Add Tool to Unit's Tool Slot (transform)
            Instantiate(lastToolStationToReach.toolPrefab, toolSlot);
            equippedTool = lastToolStationToReach.toolPrefab.GetComponent<Tool>();
        }

        private void UPDATE_GetCarComponent()
        {
            Debug.Log("UPDATE_GetCarComponent");
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
