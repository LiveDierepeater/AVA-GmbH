using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;

namespace Task
{
    public class RepairTimer : MonoBehaviour
    {
        public NavMeshAgent unitToRepairAgent;
        public SelectableUnit unit;
    
        public float remainingRepairTime = 3f;

        private GoKart currentGoKart;
        private CarComponent thisCarComponent;
        private GameManager gameManager;

        private void Awake()
        {
            thisCarComponent = GetComponent<CarComponent>();
        }

        private void NewGoKartReference()
        {
            currentGoKart = TaskManager.Instance.currentGoKart;
        }

        private void Start()
        {
            unit = unitToRepairAgent.GetComponent<SelectableUnit>();
            
            // Initializing GameManager Reference for OnNextGoKart Event.
            gameManager = TaskManager.Instance.gameManager;
            gameManager.OnNextGoKart += NewGoKartReference;
            
            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;
            
            // Play Repair-SFX.
            unitToRepairAgent.GetComponent<SelectableUnit>().PlayRepairSFX();
        }

        private void Update()
        {
            // Return if Unit is not in range.
            if (!currentGoKart.IsUnitInRange(unitToRepairAgent) ||
                unit.currentState != SelectableUnit.States.RepairKart) return;

            // Tick time.
            TickTimer();
        }

        private void TickTimer()
        {
            switch (remainingRepairTime)
            {
                case > 0:
                    remainingRepairTime -= Time.deltaTime;
                    break;
            
                case < 0:
                    remainingRepairTime = 0;
                    thisCarComponent.status = CarComponent.Status.Intact;
                    UPDATE_GoKartLists();
                    KillRepairTimerComponent();
                    break;
            }
        }

        private void UPDATE_GoKartLists()
        {
            // Removes the damagedPart from Damaged-List and Adds it to Intact-List.
            currentGoKart.damagedParts.Remove(thisCarComponent);
            currentGoKart.intactParts.Add(thisCarComponent);
            gameManager.UPDATE_RemoveDamagedPart(thisCarComponent);
        
            // Change Units state to Idle.
            unitToRepairAgent.GetComponent<SelectableUnit>().currentState = SelectableUnit.States.Idle;
        }

        private void KillRepairTimerComponent()
        {
            Destroy(this);
        }

        public void SetUnitToRepair(NavMeshAgent newUnitToRepairAgent)
        {
            unitToRepairAgent = newUnitToRepairAgent;
        }
    }
}
