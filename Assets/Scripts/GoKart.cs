using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoKart : MonoBehaviour
{
    public CarComponent[] carComponents;

    public List<CarComponent> brokenParts;
    public List<CarComponent> damagedParts;
    public List<CarComponent> intactParts;

    public bool debugCarComponents;
    
    private void Start()
    {
        RollCarComponentsStatus();
        ListInOrderOfStatus();

        DEBUG_CarComponents();
    }

    private void RollCarComponentsStatus()
    {
        foreach (CarComponent carComponent in carComponents)
        {
            int newStatus = Random.Range(0, 3);

            carComponent.status = newStatus switch
            {
                0 => CarComponent.Status.Broken,
                1 => CarComponent.Status.Damaged,
                2 => CarComponent.Status.Intact,
                _ => carComponent.status
            };
        }
    }

    private void ListInOrderOfStatus()
    {
        foreach (CarComponent part in carComponents)
        {
            if (part.status == CarComponent.Status.Broken)
            {
                brokenParts.Add(part);
            }
            else if (part.status == CarComponent.Status.Damaged)
            {
                damagedParts.Add(part);
            }
            else
            {
                intactParts.Add(part);
            }
        }
    }

    private void DEBUG_CarComponents()
    {
        if (!debugCarComponents) return;
        
        foreach (CarComponent carComponent in brokenParts)
        {
            Debug.Log(carComponent.carPart + ", " + carComponent.status);
        }
        foreach (CarComponent carComponent in damagedParts)
        {
            Debug.Log(carComponent.carPart + ", " + carComponent.status);
        }
        foreach (CarComponent carComponent in intactParts)
        {
            Debug.Log(carComponent.carPart + ", " + carComponent.status);
        }
    }
}
