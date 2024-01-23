using System;
using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;
using Task;

public class RemovingTimer : MonoBehaviour
{
    public NavMeshAgent unitToRemoveCarComponentAgent;
    
    public float remainingRemovingTime = 3f;

    private GoKart currentGoKart;
    private CarComponent thisCarComponent;
    private GameManager gameManager;
    private SelectableUnit unitToRemoveCarComponent;

    private void Awake()
    {
        thisCarComponent = GetComponent<CarComponent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        // Initializing GoKart Reference.
        currentGoKart = TaskManager.Instance.currentGoKart;
    }

    private void Update()
    {
        // Return if Unit is not in range.
        if (!currentGoKart.IsUnitInRange(unitToRemoveCarComponentAgent)) return;

        // Tick time.
        TickTimer();
    }

    private void TickTimer()
    {
        switch (remainingRemovingTime)
        {
            case > 0:
                remainingRemovingTime -= Time.deltaTime;
                break;
            
            case < 0:
                remainingRemovingTime = 0;
                UPDATE_RemovedCarComponent();
                KillRemovingTimerComponent();
                break;
        }
    }

    private void UPDATE_RemovedCarComponent()
    {
        // Goes Through Array currentGoKart.carComponents[] and finds the CarComponent which should be removed.
        // Sets the found CarComponent in Array currentGoKart.carComponents[] to Null.
        for (int i = 0; i < currentGoKart.carComponents.Length; i++)
            if (currentGoKart.carComponents[i] == thisCarComponent)
                currentGoKart.carComponents[i] = null;
            
        // Removes CarComponent from List<CarComponent> brokenParts.
        currentGoKart.brokenParts.Remove(thisCarComponent);

        // Sets Mesh-Transform of CarComponent under units ToolSlot.transform.
        thisCarComponent.transform.SetParent(unitToRemoveCarComponentAgent.transform.Find("Component Slot"));
        thisCarComponent.transform.localPosition = Vector3.zero;
            
        // Sets reference to removed and now equipped CarComponent.
        unitToRemoveCarComponentAgent.GetComponent<SelectableUnit>().equippedCarComponent = thisCarComponent;
        gameManager.UPDATE_RemovedCarComponentUI(thisCarComponent);
        
        // Change Units state to Idle.
        unitToRemoveCarComponentAgent.GetComponent<SelectableUnit>().currentState = SelectableUnit.States.Idle;
    }

    private void KillRemovingTimerComponent()
    {
        // Update Units UI.
        unitToRemoveCarComponentAgent.GetComponent<SelectableUnit>().UPDATE_UnitUI();
        
        Destroy(this);
    }

    public void SetUnitToRemoveCarComponent(NavMeshAgent newUnitToRemoveAgent)
    {
        unitToRemoveCarComponentAgent = newUnitToRemoveAgent;
        unitToRemoveCarComponent = unitToRemoveCarComponentAgent.GetComponent<SelectableUnit>();
    }
}
