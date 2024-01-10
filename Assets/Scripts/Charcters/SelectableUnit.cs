using System;
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
            
            // TODO: Implement Repair Kart Mechanic
            
            
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
                if (toolSlot.transform.GetChild(0).GetComponent<Tool>().toolType == Tool.Type.Wrench)
                    Destroy(toolSlot.transform.GetChild(0).gameObject);
                return;
            }
            
            // Add Tool to Unit's Tool Slot (transform)
            Instantiate(lastToolStationToReach.toolPrefab, toolSlot);
        }

        private void RepairCarComponents()
        {
            if (toolSlot.childCount == 0) return;                           // Checks if unit has tool
            
            if (TaskManager.Instance.damagedParts.Count == 0) return;       // Checks if car has damaged parts

            CarComponent currentPart = currentGoKart.damagedParts.ToArray()[0];
            
            currentGoKart.damagedParts.Remove(currentPart);
            currentGoKart.intactParts.Add(currentPart);
            TaskManager.Instance.RemoveDamagedPart(currentPart);
        }
    }
}
