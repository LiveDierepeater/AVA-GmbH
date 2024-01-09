using UnityEngine;
using Random = UnityEngine.Random;

public class GoKart : MonoBehaviour
{
    public CarComponent[] carComponents;

    private void Start()
    {
        RollCarComponentsStatus();
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
            
            Debug.Log(carComponent.carPart + " " + carComponent.status);
        }
    }
}
