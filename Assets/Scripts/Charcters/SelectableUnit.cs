using System;
using Karts;
using Task;
using UnityEngine;
using UnityEngine.AI;

namespace Charcters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SelectableUnit : MonoBehaviour
    {
        public SpriteRenderer selectionSprite;
        
        private NavMeshAgent agent;
        private ToolStation lastToolStationToReach;
        private GoKart currentGoKart;
        
        private Transform toolSlot;
        private Tool equippedTool;

        public enum States
        {
            Idle,
            MoveToDestination,
            GetTool,
            RepairKart
        }
        
        public States currentState;
        
        private void Awake()
        {
            SelectionManager.Instance.AvailableUnits.Add(this);
            agent = GetComponent<NavMeshAgent>();
            currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            toolSlot = transform.Find("Tool Slot");
            
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

        public void RepairKart(Vector3 newDestination)
        {
            currentState = States.RepairKart;
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
                    if (agent.remainingDistance <= agent.stoppingDistance) // Checks if unit reached tool
                    {
                        GrabTool();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.RepairKart:
                    if (agent.remainingDistance <= agent.stoppingDistance)  // Checks if unit reached Kart
                    {
                        RepairCarComponents();
                        currentState = States.Idle;
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void GrabTool()
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

        private void RepairCarComponents()
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
    }
}
