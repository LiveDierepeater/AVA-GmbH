using UnityEngine;
using UnityEngine.AI;
using Karts;
using Task;

public class RepairTimer : MonoBehaviour
{
    public NavMeshAgent unitToRepairAgent;
    
    public float remainingRepairTime = 3f;

    private GoKart currentGoKart;
    private CarComponent thisCarComponent;

    private void Awake()
    {
        currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
        thisCarComponent = GetComponent<CarComponent>();
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
        TaskManager.Instance.RemoveDamagedPart(thisCarComponent);
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
