using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SelectableUnit : MonoBehaviour
{
    private NavMeshAgent agent;
    public SpriteRenderer selectionSprite;

    private void Awake()
    {
        SelectionManager.Instance.AvailableUnits.Add(this);
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToDestination(Vector3 newDestination)
    {
        agent.SetDestination(newDestination);
    }

    public void OnSelected()
    {
        selectionSprite.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        selectionSprite.gameObject.SetActive(false);
    }
}
