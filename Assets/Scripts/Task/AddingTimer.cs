using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;

namespace Task
{
    public class AddingTimer : MonoBehaviour
    {
        public NavMeshAgent unitToAddCarComponent;
        public SelectableUnit unit;
    
        public float remainingAddingTime = 3f;

        private GoKart currentGoKart;
        private CarComponent thisCarComponent;
        private GameManager gameManager;

        private void Awake()
        {
            thisCarComponent = GetComponent<CarComponent>();
        }

        private void NewGoKartReference()
        {
            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;
        }

        private void Start()
        {
            unit = unitToAddCarComponent.GetComponent<SelectableUnit>();
            
            // Initializing GameManager Reference for OnNextGoKart Event.
            gameManager = TaskManager.Instance.gameManager;
            gameManager.OnNextGoKart += NewGoKartReference;

            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;
            
            // Play Repair-SFX.
            unitToAddCarComponent.GetComponent<SelectableUnit>().PlayRepairSFX();
        }

        private void Update()
        {
            // Return if Unit is not in range.
            if (!currentGoKart.IsUnitInRange(unitToAddCarComponent) ||
                unit.currentState != SelectableUnit.States.AddCarComponent) return;

            // Tick time.
            TickTimer();
        }

        private void TickTimer()
        {
            switch (remainingAddingTime)
            {
                case > 0:
                    remainingAddingTime -= Time.deltaTime;
                    break;
            
                case < 0:
                    remainingAddingTime = 0;
                    UPDATE_AddingCarComponent();
                    KillAddingTimerComponent();
                    break;
            }
        }

        private void UPDATE_AddingCarComponent()
        {
            // Add Car Component to Array carComponents[] & List<CarComponent> intactComponents.
            currentGoKart.carComponents[currentGoKart.GetFreeCarComponentSlotIndex()] = thisCarComponent;
            currentGoKart.intactParts.Add(thisCarComponent);

            // Change CarComponent transform to currentGoKart in localSpace.zero.
            thisCarComponent.transform.SetParent(currentGoKart.transform);
            thisCarComponent.transform.localPosition = Vector3.zero;
            thisCarComponent.transform.localRotation = Quaternion.identity;
            thisCarComponent.transform.localScale = Vector3.one;
            
            // Makes CarComponent invisible.
            thisCarComponent.GetComponentInChildren<MeshRenderer>().enabled = false;
            
            // Eliminating Reference to equippedCarComponent, as it isn't in hand anymore.
            unitToAddCarComponent.GetComponent<SelectableUnit>().equippedCarComponent = null;
            gameManager.UPDATE_RemoveBrokenPart(thisCarComponent);
            TaskManager.Instance.AddedCarComponent();
            
            // Set Units State to Idle.
            unitToAddCarComponent.GetComponent<SelectableUnit>().currentState = SelectableUnit.States.Idle;
        }

        private void KillAddingTimerComponent()
        {
            // Update Units UI.
            unitToAddCarComponent.GetComponent<SelectableUnit>().UPDATE_UnitUI();
            
            Destroy(this);
        }

        public void SetUnitToAddCarComponent(NavMeshAgent newUnitToAddAgent)
        {
            unitToAddCarComponent = newUnitToAddAgent;
        }
    }
}
