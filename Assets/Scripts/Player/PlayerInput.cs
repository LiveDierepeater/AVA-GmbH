using System.Linq;
using UnityEngine;
using Characters;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private new Camera camera;
        [SerializeField] private RectTransform selectionBox;
        [SerializeField] private LayerMask unitLayers;
        [SerializeField] private LayerMask groundToolComponentLayerMask;
        [SerializeField] private float dragDelay = 0.1f;
        
        public Texture2D mouseCursor;

        private float mouseDownTime;
        private Vector2 startMousePosition;
        
        public GameObject moveToSpritePrefab;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            ChangeMouseCursor();
        }

        private void Update()
        {
            HandleSelectionInputs();
            HandleMovementInputs();
            HandleKeyInputs();
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
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, groundToolComponentLayerMask))
                {
                    // If we clicked on a Tool.
                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Tools")
                    {
                        foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                            unit.GetTool(hit.point, hit.transform);
                    }
                
                    // If we clicked on a Kart.
                    else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Kart")
                    {
                        // If one selected unit has no equipped Tool and wants to Remove or Add CarComponent.
                        if (SelectionManager.Instance.SelectedUnits.Count == 1)
                        {
                            // If no Tool equipped nor CarComponent.
                            if (SelectionManager.Instance.SelectedUnits.ToArray()[0].toolSlot.childCount == 0 &&
                                SelectionManager.Instance.SelectedUnits.ToArray()[0].componentSlot.childCount == 0)
                            {
                                SelectionManager.Instance.SelectedUnits.ToArray()[0].RemoveCarComponent(hit.point);
                            }
                            
                            // If unit only has a CarComponent and not a Tool equipped.
                            else if (SelectionManager.Instance.SelectedUnits.ToArray()[0].componentSlot.childCount == 1 &&
                                     SelectionManager.Instance.SelectedUnits.ToArray()[0].toolSlot.childCount == 0)
                            {
                                SelectionManager.Instance.SelectedUnits.ToArray()[0].AddCarComponent(hit.point);
                            }
                            
                            // If unit only has a Tool and not a CarComponent equipped.
                            else if (SelectionManager.Instance.SelectedUnits.ToArray()[0].toolSlot.childCount == 1 &&
                                     SelectionManager.Instance.SelectedUnits.ToArray()[0].componentSlot.childCount == 0)
                            {
                                SelectionManager.Instance.SelectedUnits.ToArray()[0].RepairKart(hit.point);
                            }
                        }
                        
                        // If there are multiple units who want to repair things.
                        else
                        {
                            foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                            {
                                // Checks if Unit has only a Tool in hand.
                                if (unit.toolSlot.childCount != 0 && unit.componentSlot.childCount == 0)
                                    unit.RepairKart(hit.point);
                            }
                        }
                    }
                    
                    // If we clicked on a CarComponent.
                    else if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "CarComponents")
                    {
                        foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                            unit.GetCarComponent(hit.point, hit.transform);
                    }
                    
                    // If we clicked on the Ground.
                    else
                    {
                        foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                        {
                            unit.MoveToDestination(hit.point);
                            SpawnMoveToSprite(hit.point);
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

            foreach (var availableUnit in SelectionManager.Instance.AvailableUnits)
            {
                if (UnitIsInSelectionBox(camera.WorldToScreenPoint(availableUnit.transform.position), bounds))
                    SelectionManager.Instance.Select(availableUnit);
                else
                    SelectionManager.Instance.Deselect(availableUnit);
            }
        }

        private bool UnitIsInSelectionBox(Vector3 position, Bounds bounds)
        {
            return position.x > bounds.min.x
                   && position.x < bounds.max.x
                   && position.y > bounds.min.y
                   && position.y < bounds.max.y;
        }

        private void HandleKeyInputs()
        {
            if (Input.GetKeyUp(KeyCode.S) && SelectionManager.Instance.SelectedUnits.Count > 0)
            {
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, groundToolComponentLayerMask))
                {
                    // If Mouse is not hovering over the ground -> Drops-Off the Item directly at current Position.
                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) != "Ground")
                        foreach (SelectableUnit selectedUnit in SelectionManager.Instance.SelectedUnits)
                        {
                            selectedUnit.DropOffItem(selectedUnit.transform.position);
                        }
                    
                    // If Mouse is hovering over the ground -> Unit walks to new Destination and Drops the Item there.
                    else
                        foreach (SelectableUnit selectedUnit in SelectionManager.Instance.SelectedUnits)
                            if (selectedUnit.DoesUnitHaveAnythingInHand())
                            {
                                selectedUnit.DropOffItem(hit.point);
                                SpawnMoveToSprite(hit.point);
                            }
                }
            }
        }

        private void SpawnMoveToSprite(Vector3 destination)
        {
            GameObject newMoveToSprite = Instantiate(moveToSpritePrefab, destination, Quaternion.identity);
            newMoveToSprite.transform.position = destination + new Vector3(0f, 0.05f, 0f);
            Destroy(newMoveToSprite, 1f);
        }

        private void ChangeMouseCursor()
        {
            Cursor.SetCursor(mouseCursor, new Vector2(0, 0), CursorMode.Auto);
        }
    }
}
