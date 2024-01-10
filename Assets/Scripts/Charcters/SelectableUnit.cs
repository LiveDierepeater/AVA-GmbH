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
        private Tool lastToolToReach;

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
            lastToolToReach = toolToReach.GetComponent<Tool>();
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
                        CheckForToolSlot();
                        GrabTool();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.RepairKart:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void CheckForToolSlot()
        {
            // TODO: Check if Unit already has a tool in hand.
        }

        private void GrabTool()
        {
            // TODO: Add Tool to Unit's Tool Slot (transform)
            Debug.Log("Tool grabbed");
        }
    }
}
