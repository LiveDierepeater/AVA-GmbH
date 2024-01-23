using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;

namespace Task
{
    public class RepairTimer : MonoBehaviour
    {
        public NavMeshAgent unitToRepairAgent;
    
        public float remainingRepairTime = 3f;

        private GoKart currentGoKart;
        private CarComponent thisCarComponent;
        private GameManager gameManager;

        private void Awake()
        {
            currentGoKart = TaskManager.Instance.currentGoKart;
            thisCarComponent = GetComponent<CarComponent>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void Update()
        {
            // Return if Unit is not in range.
            if (!currentGoKart.IsUnitInRange(unitToRepairAgent)) return;

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
