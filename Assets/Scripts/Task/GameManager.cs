using UnityEngine;
using Karts;
using TMPro;
using UI;

namespace Task
{
    public class GameManager : MonoBehaviour
    {
        public CarComponentsListUI carComponentsLeftListUI;
        public CarComponentsListUI carComponentsRightListUI;
        public TextMeshProUGUI moneyCounterUI;

        private readonly string moneyCounterUILabel = "Money: ";
        private int currentMoneyAmount;

        [Header("GoKartTarget")] public Transform goKartTargetDestination;

        [Header("GoKartPrefab")] public GameObject goKartPrefab;
        
        public delegate void ENextGoKart();
        public event ENextGoKart OnNextGoKart;

        public void UPDATE_RemoveDamagedPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveDamagedPart(carComponent);
            carComponentsRightListUI.RemoveDamagedCarComponentsFromUIList(carComponent);
            currentMoneyAmount += 10;
            moneyCounterUI.text = moneyCounterUILabel + currentMoneyAmount;
        }

        public void UPDATE_RemoveBrokenPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveBrokenPart(carComponent);
            carComponentsLeftListUI.RemoveBrokenCarComponentFromUIList(carComponent);
            currentMoneyAmount += 10;
            moneyCounterUI.text = moneyCounterUILabel + currentMoneyAmount;
        }

        public void UPDATE_RemovedCarComponentUI(CarComponent carComponent)
        {
            carComponentsLeftListUI.UPDATE_LooseCarComponentUI(carComponent);
        }

        private void Awake()
        {
            TaskManager.Instance.gameManager = this;
            
            TaskManager.Instance.currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            TaskManager.Instance.OnGoKartFinished += TaskManager_OnGoKartFinished;
        }

        private void TaskManager_OnGoKartFinished()
        {
            // Drive current GoKart out of Garage.
            TaskManager.Instance.currentGoKart.goKartStatus = GoKart.Status.DrivingOut;
            
            // TODO: Update UI.
        }

        public void NextGoKart()
        {
            // Kills finished GoKart.
            Destroy(TaskManager.Instance.currentGoKart.gameObject);

            // Creates new GoKart.
            GameObject newGoKart = Instantiate(goKartPrefab, new Vector3(0, 0, 15), Quaternion.identity);
            TaskManager.Instance.currentGoKart = newGoKart.GetComponent<GoKart>();
            
            // Refresh GoKart references.
            OnNextGoKart?.Invoke();
        }
    }
}
