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

        private Tool newTool;
        private CarComponent newCarComponent;

        private Vector3 currentNewDestination;

        public enum States
        {
            Idle,
            MoveToDestination,
            GetTool,
            GetCarComponent,
            RepairKart,
            RemoveCarComponent,
            AddCarComponent,
            DropOffItem
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
            newCarComponent = componentToReach.GetComponent<CarComponent>();

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

        public void DropOffItem(Vector3 newDestination)
        {
            currentState = States.DropOffItem;
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
                
                case States.DropOffItem:
                    if (DoesUnitReachedDestination(distanceToDestination))
                    {
                        UPDATE_DropOffItem();
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
            equippedTool = Instantiate(lastToolStationToReach.toolPrefab, toolSlot).GetComponent<Tool>();
        }

        private void UPDATE_GrabCarComponent()
        {
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
            
            // Check if CarComponent is taken from Ground (and not from Station).
            if (newCarComponent != null)
            {
                // Return if CarComponent was taken by another Unit.
                if (newCarComponent.transform.parent != null) return;
                
                // Destroy Rigidbody to grab CarComponent Properly
                // Takes newCarComponent.transform as a Child-Object into his componentSlot.
                Destroy(newCarComponent.gameObject.GetComponent<Rigidbody>());
                newCarComponent.transform.SetParent(componentSlot);
                newCarComponent.transform.localPosition = Vector3.zero;
                newCarComponent.transform.localRotation = Quaternion.identity;
                equippedCarComponent = newCarComponent.GetComponent<CarComponent>();
                return;
            }
            
            // Add CarComponent to Unit's Component Slot (transform)
            equippedCarComponent = 
                Instantiate(lastComponentStationToReach.carComponentPrefab, componentSlot).GetComponent<CarComponent>();
            equippedCarComponent.status = CarComponent.Status.Intact;
        }

        private void UPDATE_RepairCarComponents()
        {
            if (toolSlot.childCount == 0) return;                           // Checks if unit has tool
            
            if (TaskManager.Instance.damagedParts.Count == 0) return;              // Checks if car has damaged parts
            
            CarComponent partToRepair = null;                               // Creates place for partToRepair
            
            // Goes through all damaged parts.
            // The last damaged part which toolToGetRepaired matches the equipped tool gets saved.
            foreach (CarComponent damagedPart in TaskManager.Instance.damagedParts)
                if (damagedPart.toolToRepair.toolType == equippedTool.toolType)
                    partToRepair = damagedPart;

            // If there is no part which could get repaired witch the currently equipped tool -> return.
            if (partToRepair is null) return;
            
            // Return if there is already a RepairTimer Component on the partToRepair.
            if (partToRepair.TryGetComponent(out RepairTimer existingRepairTimer)) return;
            
            // Timer which simulates the time an Unit needs to repair a CarComponent with Tool.
            RepairTimer repairTimer = partToRepair.gameObject.AddComponent<RepairTimer>();
            repairTimer.SetUnitToRepair(agent);
            
            // TODO: Should be initialized when finished with Repair-Time.
            // Removes the damagedPart from Damaged-List and Adds it to Intact-List.
            // currentGoKart.damagedParts.Remove(partToRepair);
            // currentGoKart.intactParts.Add(partToRepair);
            // TaskManager.Instance.RemoveDamagedPart(partToRepair);
        }

        private void UPDATE_RemoveCarComponent()
        {
            // TODO: QuickTime-Event
            Debug.Log("QuickTime-Event");

            // Set the first CarComponent in Array currentGoKart.carComponents[] to Null.
            // Remove Car Component from List<CarComponent> brokenComponents.
            CarComponent carComponentToRemove = currentGoKart.brokenParts[0];
            equippedCarComponent = carComponentToRemove;

            // Goes Through Array currentGoKart.carComponents[] and finds the CarComponent which should be removed.
            // Sets the found CarComponent to Null.
            for (int i = 0; i < currentGoKart.carComponents.Length; i++)
                if (currentGoKart.carComponents[i] == carComponentToRemove)
                    currentGoKart.carComponents[i] = null;
            
            // Sets the CarComponent which will get removed to Null.
            currentGoKart.brokenParts.Remove(carComponentToRemove);

            // Sets Mesh-Transform of CarComponent under units ToolSlot.transform.
            equippedCarComponent.transform.SetParent(transform.Find("Component Slot"));
            equippedCarComponent.transform.localPosition = Vector3.zero;
        }

        private void UPDATE_AddCarComponent()
        {
            // TODO: QuickTime-Event
            
            // Return if equippedCarComponent matches a CarCoponent in List<CarComponent> carComponents.
            if (currentGoKart.CheckForDoubledCarComponents(equippedCarComponent)) return;
            
            // Return if List<CarComponent> carComponents has no free slots.
            if (currentGoKart.GetFreeCarComponentSlotIndex() < 0) return;
            
            // Return if equippedCarComponent is not intact.
            if (equippedCarComponent.status != CarComponent.Status.Intact) return;

            // Add Car Component to List<CarComponent> carComponents & List<CarComponent> intactComponents.
            currentGoKart.carComponents[currentGoKart.GetFreeCarComponentSlotIndex()] = equippedCarComponent;
            currentGoKart.intactParts.Add(equippedCarComponent);

            // Change CarComponent transform to currentGoKart in localspace.zero.
            equippedCarComponent.transform.SetParent(currentGoKart.transform);
            equippedCarComponent.transform.localPosition = Vector3.zero;
            equippedCarComponent.transform.localRotation = Quaternion.identity;
            equippedCarComponent.transform.localScale = Vector3.one;
            
            // Eliminating Reference to equippedCarComponent, as it isn't in hand anymore.
            equippedCarComponent = null;
        }

        private void UPDATE_DropOffItem()
        {
            // If Unit has a Tool in Hand:
            // Un-Parents the Tool -> Activates Rigidbody -> Sets equippedTool to Null.
            if (equippedTool != null)
            {
                Transform toolToDrop = toolSlot.GetChild(0);
                toolToDrop.SetParent(null);
                
                // Activate Rigidbody
                Rigidbody newRigidbody = toolToDrop.gameObject.AddComponent<Rigidbody>();
                newRigidbody.mass = 30;

                equippedTool = null;
            }
            
            // If Unit has a CarComponent in Hand:
            // Un-Parents the CarComponent -> Activates Rigidbody -> Sets equippedCarComponent to Null.
            else if (equippedCarComponent != null)
            {
                Transform componentToDrop = componentSlot.GetChild(0);
                componentToDrop.SetParent(null);
                
                // Activate Rigidbody
                Rigidbody newRigidbody = componentToDrop.gameObject.AddComponent<Rigidbody>();
                newRigidbody.mass = 30;
                
                equippedCarComponent = null;
            }
        }

        public bool DoesUnitHaveAnythingInHand()
        {
            return toolSlot.childCount != 0 || componentSlot.childCount != 0;
        }
    }
}
