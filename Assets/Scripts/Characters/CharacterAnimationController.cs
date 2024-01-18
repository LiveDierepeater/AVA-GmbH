using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator unitAnimator;
    
    private SelectableUnit unit;
    private NavMeshAgent unitAgent;
    private GoKart currentGoKart;
    
    private float velocity;
    private bool hasUnitItemInHand;
    private bool isUnitRepairing;
    
    private void Awake()
    {
        unit = GetComponent<SelectableUnit>();
        unitAgent = GetComponent<NavMeshAgent>();
        currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
    }

    private void Update()
    {
        if (unit.DoesUnitHaveAnythingInHand())
        {
            // Units Animation with Item in Hand.
            hasUnitItemInHand = true;
        }
        else
        {
            // Units Animation without Item in hand.
            hasUnitItemInHand = false;
        }

        if (currentGoKart.IsUnitInRange(unitAgent))
        {
            // 
            if (unit.currentState == SelectableUnit.States.RepairKart ||
                unit.currentState == SelectableUnit.States.AddCarComponent ||
                unit.currentState == SelectableUnit.States.RemoveCarComponent)
            {
                isUnitRepairing = true;
            }
            else
            {
                isUnitRepairing = false;
            }
        }
        
        velocity = unitAgent.velocity.magnitude / 10f;
        unitAnimator.SetFloat("Speed", velocity);
        unitAnimator.SetBool("IsItemEquipped", hasUnitItemInHand);
        unitAnimator.SetBool("IsRepairing", isUnitRepairing);
    }
}
