using UnityEngine;
using Karts;
using TMPro;
using UI;
using UnityEngine.Serialization;

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
            TaskManager.Instance.currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
            TaskManager.Instance.OnGokartFinished += Taskmanager_OnGoKartFinished;
        }

        public void Taskmanager_OnGoKartFinished()
        {
            // TODO: Drive current GoKart out of Garage.
            TaskManager.Instance.currentGoKart.DriveToDestination(goKartTargetDestination.position);

            // TODO: Kill current GoKart.

            // TODO: Create new GoKart.

            // TODO: Replace old GoKart References with new GoKart.

            // TODO: Update UI.
        }
    }
}
