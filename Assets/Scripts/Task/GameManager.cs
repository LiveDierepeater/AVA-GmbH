using Characters;
using UnityEngine;
using Karts;
using UI;

namespace Task
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        public CarComponentsListUI carComponentsLeftListUI;
        public CarComponentsListUI carComponentsRightListUI;

        private MoneyEconomy moneyEconomy;

        [Header("Unit Room Indicator")] public Transform unitRoomIndicator;

        [Header("GoKartTarget")] public Transform goKartTargetDestination;

        [Header("GoKartPrefab")] public GameObject goKartPrefab;
        
        [Header("Tutorial GoKart")] public GameObject tutorialKart;
        
        public delegate void ENextGoKart();
        public event ENextGoKart OnNextGoKart;

        public void UPDATE_RemoveDamagedPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveDamagedPart(carComponent);
            carComponentsRightListUI.RemoveDamagedCarComponentsFromUIList(carComponent);
            moneyEconomy.RemoveDamagedPart();
        }

        public void UPDATE_RemoveBrokenPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveBrokenPart(carComponent);
            carComponentsLeftListUI.RemoveBrokenCarComponentFromUIList(carComponent);
            moneyEconomy.RemoveBrokenPart();
        }

        public void UPDATE_RemovedCarComponentUI(CarComponent carComponent)
        {
            carComponentsLeftListUI.UPDATE_LooseCarComponentUI(carComponent);
        }

        private void Awake()
        {
            TaskManager.Instance.gameManager = this;
            
            SelectionManager.Instance.AvailableUnits.Clear();
            
            TaskManager.Instance.currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            TaskManager.Instance.OnGoKartFinished += TaskManager_OnGoKartFinished;

            moneyEconomy = GetComponentInChildren<MoneyEconomy>();
        }

        private void FixedUpdate()
        {
            foreach (SelectableUnit availableUnit in SelectionManager.Instance.AvailableUnits)
            {
                SetUnitsRoomLocation(availableUnit);
                
                if (availableUnit.unitUIBorder.color != availableUnit.unitUIColor)
                    availableUnit.unitUIBorder.color = availableUnit.unitUIColor;
            }
            
            foreach (SelectableUnit selectedUnit in SelectionManager.Instance.SelectedUnits)
            {
                SetUnitsRoomLocation(selectedUnit);
                
                if (selectedUnit.unitUIBorder.color != Color.white)
                    selectedUnit.unitUIBorder.color = Color.white;
            }
        }

        private void SetUnitsRoomLocation(SelectableUnit unit)
        {
            if (unit.transform.position.x < unitRoomIndicator.transform.position.x)
                unit.unitsRoomLocation = SelectableUnit.Room.Garage;
            else
                unit.unitsRoomLocation = SelectableUnit.Room.Storage;
        }

        private void TaskManager_OnGoKartFinished()
        {
            // Drive current GoKart out of Garage.
            TaskManager.Instance.currentGoKart.goKartStatus = GoKart.Status.DrivingOut;
            
            // Calls goKart DrivingOut Sound in GoKart.
            TaskManager.Instance.currentGoKart.PlayDrivingOutSFX();
        }

        public void NextGoKart()
        {
            bool isTutorialGoKart = TaskManager.Instance.currentGoKart.isTutorialGoKart;
            
            // Kills finished GoKart.
            Destroy(TaskManager.Instance.currentGoKart.gameObject);

            // Creates new GoKart.
            if (!isTutorialGoKart)
            {
                GameObject newGoKart = Instantiate(goKartPrefab, new Vector3(0, 0, 15), Quaternion.identity);
                TaskManager.Instance.currentGoKart = newGoKart.GetComponent<GoKart>();
            }
            else
            {
                GameObject newGoKart = Instantiate(tutorialKart, new Vector3(0, 0, 15), Quaternion.identity);
                TaskManager.Instance.currentGoKart = newGoKart.GetComponent<GoKart>();
            }
            
            // Refresh GoKart references.
            OnNextGoKart?.Invoke();
        }
    }
}
