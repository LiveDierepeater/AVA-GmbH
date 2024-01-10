using Charcters;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private LayerMask unitLayers;
    [SerializeField] private LayerMask goundAndToolLayerMask;
    [SerializeField] private float dragDelay = 0.1f;

    private float mouseDownTime;
    private Vector2 startMousePosition;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleSelectionInputs();
        HandleMovementInputs();
    }

    private void HandleSelectionInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(true);
            startMousePosition = Input.mousePosition;
            mouseDownTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Mouse0) && mouseDownTime + dragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(false);

            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, unitLayers)
                && hit.collider.TryGetComponent(out SelectableUnit unit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(unit))
                    {
                        SelectionManager.Instance.Deselect(unit);
                    }
                    else
                    {
                        SelectionManager.Instance.Select(unit);
                    }
                }
                else
                {
                    SelectionManager.Instance.DeselectAll();
                    SelectionManager.Instance.Select(unit);
                }
            }
            else if (mouseDownTime + dragDelay > Time.time)
            {
                SelectionManager.Instance.DeselectAll();
            }
                
            mouseDownTime = 0;
        }
    }

    private void HandleMovementInputs()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, goundAndToolLayerMask))
            {
                // If we clicked on a tool.
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Tools")
                {
                    foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                    {
                        unit.GetTool(hit.point, hit.transform);
                    }
                }
                else
                {
                    foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                    {
                        unit.MoveToDestination(hit.point);
                    }
                }
            }
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - startMousePosition.x;
        float height = Input.mousePosition.y - startMousePosition.y;

        selectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
            if (UnitIsInSelectionBox(
                    camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
            {
                SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
                
            }
            else SelectionManager.Instance.Deselect(SelectionManager.Instance.AvailableUnits[i]);
        }
    }

    private bool UnitIsInSelectionBox(Vector3 position, Bounds bounds)
    {
        return position.x > bounds.min.x
               && position.x < bounds.max.x
               && position.y > bounds.min.y
               && position.y < bounds.max.y;
    }
}
