using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;

namespace Task
{
    public class AddingTimer : MonoBehaviour
    {
    
        public NavMeshAgent unitToAddCarComponent;
    
        public float remainingAddingTime = 3f;

        private GoKart currentGoKart;
        private CarComponent thisCarComponent;
        private GameManager gameManager;

        private void Awake()
        {
            currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            thisCarComponent = GetComponent<CarComponent>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void Update()
        {
            // Return if Unit is not in range.
            if (!currentGoKart.IsUnitInRange(unitToAddCarComponent)) return;

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
            
            // Eliminating Reference to equippedCarComponent, as it isn't in hand anymore.
            unitToAddCarComponent.GetComponent<SelectableUnit>().equippedCarComponent = null;
            gameManager.UPDATE_RemoveBrokenPart(thisCarComponent);
        }

        private void KillAddingTimerComponent()
        {
            Destroy(this);
        }

        public void SetUnitToAddCarComponent(NavMeshAgent newUnitToAddAgent)
        {
            unitToAddCarComponent = newUnitToAddAgent;
        }
    }
}
