using UnityEngine;
using UnityEngine.AI;
using Characters;
using Karts;
using Random = UnityEngine.Random;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator unitAnimator;
    public int randomIdlingChance;
    
    private SelectableUnit unit;
    private NavMeshAgent unitAgent;
    private GoKart currentGoKart;
    
    private float velocity;
    private bool hasUnitItemInHand;
    private bool isUnitRepairing;
    private bool isUnitRandomIdling;
    
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

        // When Unit is doing something on GoKart.
        if (currentGoKart.IsUnitInRange(unitAgent))
        {
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
        
        if (unitAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Random"))
        {
            if (unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f) isUnitRandomIdling = false;
            if (velocity >= 0.01f) isUnitRandomIdling = false;
        }
        
        velocity = unitAgent.velocity.magnitude / 10f;
        unitAnimator.SetFloat("Speed", velocity);
        unitAnimator.SetBool("IsItemEquipped", hasUnitItemInHand);
        unitAnimator.SetBool("IsRepairing", isUnitRepairing);
        unitAnimator.SetBool("IsRandomIdling", isUnitRandomIdling);
    }

    private void Start()
    {
        Invoke(nameof(RollRandomIdlingChance), 1f);
    }

    private void RollRandomIdlingChance()
    {
        int roll = Random.Range(0, 101);

        if (randomIdlingChance > roll) isUnitRandomIdling = true;

        Invoke(nameof(RollRandomIdlingChance), 1f);
    }
}