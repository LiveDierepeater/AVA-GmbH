using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Karts;
using Task;
using UI;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SelectableUnit : MonoBehaviour
    {
        public SpriteRenderer selectionSprite;
        public Sprite unitsUIAvatarSprite;

        public UnitUIController unitUIController;
        
        private NavMeshAgent agent;
        private ToolStation lastToolStationToReach;
        private ComponentStation lastComponentStationToReach;
        private GoKart currentGoKart;
        private AudioSource audioSource;

        [Header("Audio Clips")]
        public AudioClip denySFX;
        public AudioClip grabSFX;
        public AudioClip dropSFX;
        public AudioClip repairSFX;
        [Space(10)]
        
        [Header("Item Slots")]
        public Transform toolSlot;
        public Transform componentSlot;
        private Tool equippedTool;
        public CarComponent equippedCarComponent;

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
        
        public enum Room
        {
            Garage,
            Storage
        }

        public Room unitsRoomLocation;

        public Image unitUIBorder;

        public Color unitUIColor;
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            toolSlot = transform.Find("Tool Slot");
            componentSlot = transform.Find("Component Slot");
            audioSource = GetComponent<AudioSource>();
            
            currentState = States.Idle;
            unitsRoomLocation = Room.Garage;
        }

        private void NewGoKartReference()
        {
            currentGoKart = TaskManager.Instance.currentGoKart;
        }

        private void Start()
        {
            // Initialize Unit
            SelectionManager.Instance.AvailableUnits.Add(this);
            
            // Initializing GameManager Reference for OnNextGoKart Event.
            GameManager gameManager = TaskManager.Instance.gameManager;
            gameManager.OnNextGoKart += NewGoKartReference;

            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;
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
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void GetTool(Vector3 newDestination, Transform toolToReach)
        {
            currentState = States.GetTool;
            lastToolStationToReach = toolToReach.parent.GetComponent<ToolStation>();
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void GetCarComponent(Vector3 newDestination, Transform componentToReach)
        {
            currentState = States.GetCarComponent;
            
            lastComponentStationToReach = componentToReach.GetComponent<ComponentStation>();
            newCarComponent = componentToReach.GetComponent<CarComponent>();
            
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void RepairKart(Vector3 newDestination)
        {
            currentState = States.RepairKart;
            
            // Old Destination Calculation.
            //currentNewDestination = newDestination;

            currentNewDestination = GetClosestDistanceSlotToUnit().position;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void RemoveCarComponent(Vector3 newDestination)
        {
            currentState = States.RemoveCarComponent;
            
            // Old Destination Calculation.
            //currentNewDestination = newDestination;

            currentNewDestination = GetClosestDistanceSlotToUnit().position;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void AddCarComponent(Vector3 newDestination)
        {
            currentState = States.AddCarComponent;
            
            // Old Destination Calculation.
            //currentNewDestination = newDestination;

            currentNewDestination = GetClosestDistanceSlotToUnit().position;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
        }

        public void DropOffItem(Vector3 newDestination)
        {
            currentState = States.DropOffItem;
            currentNewDestination = newDestination;
            agent.SetDestination(currentNewDestination);
            
            Debug.DrawLine(new Vector3(transform.position.x, currentNewDestination.y, transform.position.z), currentNewDestination, Color.green, 3f);
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();
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
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;
                
                case States.GetTool:
                    if (DoesUnitReachedDestination(distanceToDestination)) // Checks if unit reached tool
                    {
                        UPDATE_GrabTool();
                        currentState = States.Idle;
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;
                
                case States.GetCarComponent:
                    if (DoesUnitReachedDestination(distanceToDestination)) // Checks if unit reached tool
                    {
                        UPDATE_GrabCarComponent();
                        currentState = States.Idle;
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;
                
                case States.RepairKart:
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached Kart
                    {
                        UPDATE_RepairCarComponents();
                        //currentState = States.Idle;
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;

                case States.RemoveCarComponent:
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached Kart
                    {
                        UPDATE_RemoveCarComponent();
                        //currentState = States.Idle;
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;
                
                case States.AddCarComponent :
                    if (currentGoKart.IsUnitInRange(agent)) // Checks if unit reached Kart
                    {
                        UPDATE_AddCarComponent();
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
                    }
                    break;
                
                case States.DropOffItem:
                    if (DoesUnitReachedDestination(distanceToDestination))
                    {
                        UPDATE_DropOffItem();
                        currentState = States.Idle;
            
                        // Update Units UI.
                        unitUIController.UPDATE_UnitUI();
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
                    // Lays the Tool back onto the Tool Station.
                    Destroy(toolSlot.GetChild(0).gameObject);
                    equippedTool = null;
                    
                    // Update Units UI.
                    unitUIController.ClearItemUISprite();

                    // Drop Tool SFX.
                    PlayDropSFX();
                }
                else
                    // Tool is not from this Station.
                    PlayDenySFX();
                
                return;
            }
            
            // Check if Unit already has a CarComponent in hand.
            if (componentSlot.childCount != 0)
            {
                PlayDenySFX();
                return;
            }

            // Add Tool to Unit's Tool Slot (transform)
            equippedTool = Instantiate(lastToolStationToReach.toolPrefab, toolSlot).GetComponent<Tool>();

            // Update Units UI.
            unitUIController.UPDATE_UnitUI();

            // Play Grab-SFX.
            PlayGrabSFX();
        }

        private void UPDATE_GrabCarComponent()
        {
            // Check if Unit already has a CarComponent in hand.
            if (componentSlot.childCount != 0)
            {
                // Check if equipped CarComponent is from this ComponentStation.
                if (componentSlot.GetChild(0).GetComponent<CarComponent>().carPartType ==
                    lastComponentStationToReach.carComponentPrefab.GetComponent<CarComponent>().carPartType)
                {
                    // Lays it back onto CarComponent Station.
                    Destroy(componentSlot.GetChild(0).gameObject);
                    equippedCarComponent = null;
                    
                    // Update Units UI:
                    unitUIController.ClearItemUISprite();

                    // Drop Component SFX.
                    PlayDropSFX();
                }
                else
                    // Tool is not from this Station.
                    PlayDenySFX();
                
                return;
            }
            
            // Check if Unit already has a Tool in hand.
            if (toolSlot.childCount != 0)
            {
                PlayDenySFX();
                return;
            }
            
            // Check if CarComponent is taken from Ground (and not from Station).
            if (newCarComponent is not null)
            {
                // Return if CarComponent was taken by another Unit.
                if (newCarComponent.transform.parent is not null) return;
                
                // Destroy Rigidbody to grab CarComponent Properly
                // Takes newCarComponent.transform as a Child-Object into his componentSlot.
                Destroy(newCarComponent.gameObject.GetComponent<Rigidbody>());
                newCarComponent.transform.SetParent(componentSlot);
                newCarComponent.transform.localPosition = Vector3.zero;
                newCarComponent.transform.localRotation = Quaternion.identity;
                equippedCarComponent = newCarComponent.GetComponent<CarComponent>();
                
                // Check if Component has AddingTimer.cs
                // When Component has AddingTimer.cs -> Change the navMeshAgent-Reference to this.agent.
                if (newCarComponent.TryGetComponent(out AddingTimer addingTimer))
                {
                    addingTimer.unitToAddCarComponent = agent;
                    addingTimer.unit = this;
                }
                
                // Update Units UI:
                unitUIController.UPDATE_UnitUI();
                
                // Grab Component SFX.
                PlayGrabSFX();
                
                return;
            }
            
            // Add CarComponent to Unit's Component Slot (transform)
            equippedCarComponent = 
                Instantiate(lastComponentStationToReach.carComponentPrefab, componentSlot).GetComponent<CarComponent>();
            equippedCarComponent.status = CarComponent.Status.Intact;
            
            // Update Units UI.
            unitUIController.UPDATE_UnitUI();

            // Play Grab-SFX.
            PlayGrabSFX();
        }

        private void UPDATE_RepairCarComponents()
        {
            if (toolSlot.childCount == 0) return;                           // Checks if unit has tool.
            
            if (TaskManager.Instance.damagedParts.Count == 0)               // Check if there are damaged Parts left.
            {
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            CarComponent partToRepair = null;                               // Creates place for partToRepair.
            
            // Goes through all damaged parts.
            // The last damaged part which toolToGetRepaired matches the equipped tool gets saved.
            foreach (CarComponent damagedPart in TaskManager.Instance.damagedParts)
                if (damagedPart.toolToRepair.toolType == equippedTool.toolType)
                    partToRepair = damagedPart;

            // If there is no part which could get repaired witch the currently equipped tool -> return.
            if (partToRepair is null)
            {
                // Change Units state to Idle.
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            // Return if there is already a RepairTimer.cs Component on the partToRepair.
            if (partToRepair.TryGetComponent(out RepairTimer _)) return;
            
            // Timer which simulates the time an Unit needs to repair a CarComponent with Tool.
            RepairTimer repairTimer = partToRepair.gameObject.AddComponent<RepairTimer>();
            repairTimer.SetUnitToRepair(agent);
        }

        private void UPDATE_RemoveCarComponent()
        {
            // Return if Car has no more CarComponents build in.
            if (currentGoKart.brokenParts.Count == 0)
            {
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            // Gets reference to the first CarComponent in List<CarComponent> currentGoKart.brokenParts.
            // This carComponentToRemove will get removed in future from the currentGoKart.
            CarComponent carComponentToRemove = currentGoKart.brokenParts[0];
            
            // Return if there is already a Unit removing the CarComponent...
            // ... by checking if a RemovingTimer.cs is on this carComponentToRemove.
            if (carComponentToRemove.TryGetComponent(out RemovingTimer removeTimer))
            {
                if (removeTimer.unit is null)
                {
                    removeTimer.unit = this;
                    removeTimer.unitToRemoveCarComponentAgent = agent;
                }
                
                return;
            }
            
            // Timer which simulates the time an Unit needs to remove a CarComponent.
            RemovingTimer removingTimer = carComponentToRemove.gameObject.AddComponent<RemovingTimer>();
            removingTimer.SetUnitToRemoveCarComponent(agent);
        }

        private void UPDATE_AddCarComponent()
        {
            // Return if equippedCarComponent matches a CarComponent in List<CarComponent> carComponents.
            if (currentGoKart.CheckForDoubledCarComponents(equippedCarComponent))
            {
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            // Return if List<CarComponent> carComponents has no free slots.
            if (currentGoKart.GetFreeCarComponentSlotIndex() < 0)
            {
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            // Return if equippedCarComponent is not intact.
            if (equippedCarComponent.status != CarComponent.Status.Intact)
            {
                currentState = States.Idle;
                PlayDenySFX();
                return;
            }
            
            // Return if there is already another Unit adding the currently equippedCarComponent...
            // ... by checking if an AddingTimer.cs is on this equippedCarComponent.
            if (equippedCarComponent.TryGetComponent(out AddingTimer _)) return;
            
            // Timer which simulates the time an Unit needs to add a CarComponent.
            AddingTimer addingTimer = equippedCarComponent.gameObject.AddComponent<AddingTimer>();
            addingTimer.SetUnitToAddCarComponent(agent);
        }

        private void UPDATE_DropOffItem()
        {
            // If Unit has a Tool in Hand:
            // Un-Parents the Tool -> Activates Rigidbody -> Sets equippedTool to Null.
            if (equippedTool is not null)
            {
                Transform toolToDrop = toolSlot.GetChild(0);
                toolToDrop.SetParent(null);
                
                // Activate Rigidbody
                Rigidbody newRigidbody = toolToDrop.gameObject.AddComponent<Rigidbody>();
                newRigidbody.mass = 30;

                equippedTool = null;
                
                // Play Drop-SFX.
                PlayDropSFX();
            }
            
            // If Unit has a CarComponent in Hand:
            // Un-Parents the CarComponent -> Activates Rigidbody -> Sets equippedCarComponent to Null.
            else if (equippedCarComponent is not null)
            {
                Transform componentToDrop = componentSlot.GetChild(0);
                componentToDrop.SetParent(null);
                
                // Activate Rigidbody
                Rigidbody newRigidbody = componentToDrop.gameObject.AddComponent<Rigidbody>();
                newRigidbody.mass = 30;
                
                equippedCarComponent = null;
                
                // Play Drop-SFX.
                PlayDropSFX();
            }

            unitUIController.ClearItemUISprite();
        }

        public bool DoesUnitHaveAnythingInHand()
        {
            return toolSlot.childCount != 0 || componentSlot.childCount != 0;
        }

        public Sprite GetEquippedItemUISprite()
        {
            // Return if unit has nothing in hand.
            if (!DoesUnitHaveAnythingInHand()) return null;
            
            // Return Tool.
            if (toolSlot.childCount == 1)
                return toolSlot.GetChild(0).transform.GetComponent<Tool>().toolUISprite;
            
            // Return CarComponent.
            return componentSlot.GetChild(0).transform.GetComponent<CarComponent>().carComponentUISprite;
        }

        public void SetUnitUIController(UnitUIController UI)
        {
            unitUIController = UI;
        }

        public void UPDATE_UnitUI()
        {
            unitUIController.UPDATE_UnitUI();
        }

        private Transform GetClosestDistanceSlotToUnit()
        {
            Transform closestDistanceSlot = null;
            float closestDistance = 1000;
            
            foreach (Transform distanceSlot in currentGoKart.distanceSlots)
            {
                float currentDistance = Vector3.Distance(this.transform.position, distanceSlot.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestDistanceSlot = distanceSlot;
                }
            }

            return closestDistanceSlot;
        }

        private void PlayDenySFX()
        {
            audioSource.clip = denySFX;
            audioSource.Play();
        }

        public void PlayGrabSFX()
        {
            audioSource.clip = grabSFX;
            audioSource.Play();
        }

        private void PlayDropSFX()
        {
            audioSource.clip = dropSFX;
            audioSource.Play();
        }

        public void PlayRepairSFX()
        {
            audioSource.clip = repairSFX;
            audioSource.Play();
        }
    }
}
