using Karts;
using Task;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Characters
{
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
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsItemEquipped = Animator.StringToHash("IsItemEquipped");
        private static readonly int IsRepairing = Animator.StringToHash("IsRepairing");
        private static readonly int IsRandomIdling = Animator.StringToHash("IsRandomIdling");

        private void Awake()
        {
            unit = GetComponent<SelectableUnit>();
            unitAgent = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            Invoke(nameof(RollRandomIdlingChance), 1f);
            var state = unitAnimator.GetCurrentAnimatorStateInfo(0);
            unitAnimator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
            
            // Initializing GameManager Reference for OnNextGoKart Event.
            GameManager gameManager = TaskManager.Instance.gameManager;
            gameManager.OnNextGoKart += NewGoKartReference;
            
            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;
        }

        private void NewGoKartReference()
        {
            currentGoKart = TaskManager.Instance.currentGoKart;
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
                    
                    // Update Unit UI
                    unit.unitUIController.ShowWorkingAnimationUI();
                }
                else
                {
                    isUnitRepairing = false;
            
                    // Update Unit UI
                    unit.unitUIController.HideWorkingAnimationUI();
                }
            }
        
            if (unitAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Random"))
            {
                if (unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f) isUnitRandomIdling = false;
                if (velocity >= 0.01f) isUnitRandomIdling = false;
            }
        
            velocity = unitAgent.velocity.magnitude / 10f;
            unitAnimator.SetFloat(Speed, velocity);
            unitAnimator.SetBool(IsItemEquipped, hasUnitItemInHand);
            unitAnimator.SetBool(IsRepairing, isUnitRepairing);
            unitAnimator.SetBool(IsRandomIdling, isUnitRandomIdling);
        }

        private void RollRandomIdlingChance()
        {
            int roll = Random.Range(0, 101);

            if (randomIdlingChance > roll) isUnitRandomIdling = true;

            Invoke(nameof(RollRandomIdlingChance), 1f);
        }
    }
}
