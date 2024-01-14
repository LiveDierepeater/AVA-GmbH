using UnityEngine;
using UnityEngine.AI;
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
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        public void GetTool(Vector3 newDestination, Transform toolToReach)
        {
            currentState = States.GetTool;
            lastToolStationToReach = toolToReach.GetComponent<ToolStation>();
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        public void GetCarComponent(Vector3 newDestination, Transform componentToReach)
        {
            currentState = States.GetCarComponent;
            lastComponentStationToReach = componentToReach.GetComponent<ComponentStation>();
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        public void RepairKart(Vector3 newDestination)
        {
            currentState = States.RepairKart;
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        public void RemoveCarComponent(Vector3 newDestination)
        {
            currentState = States.RemoveCarComponent;
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        public void AddCarComponent(Vector3 newDestination)
        {
            currentState = States.AddCarComponent;
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
        }

        private void Update()
        {
            float distanceToDestination = Vector3.Distance(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination);
            
            switch (currentState)
            {
                case States.Idle:
                    break;
                
                case States.MoveToDestination:
                    if (DoesUnitReachedDestination(distanceToDestination)) // Checks if unit reached tool
                    {
                        currentState = States.Idle;
                    }
                    break;
                
                case States.GetTool:
                    if (DoesUnitReachedDestination(distanceToDestination)) // Checks if unit reached tool
                    {
                        UPDATE_GrabTool();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.GetCarComponent:
                    if (DoesUnitReachedDestination(distanceToDestination)) // Checks if unit reached tool
                    {
                        UPDATE_GrabCarComponent();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.RepairKart:
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached tool
                    {
                        UPDATE_RepairCarComponents();
                        currentState = States.Idle;
                    }
                    break;

                case States.RemoveCarComponent:
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached tool
                    {
                        UPDATE_RemoveCarComponent();
                        currentState = States.Idle;
                    }
                    break;
                
                case States.AddCarComponent :
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached tool
                    {
                        UPDATE_AddCarComponent();
                        currentState = States.Idle;
                    }
                    break;
            }
        }

        private bool DoesUnitReachedDestination(float distanceToDestination)
        {
            return distanceToDestination <= agent.stoppingDistance * 1.5f;
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

        private void UPDATE_GrabCarComponent()               // TODO: TEST THIS MECHANIC
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
            Debug.Log("UPDATE_RepairCarComponents");
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
