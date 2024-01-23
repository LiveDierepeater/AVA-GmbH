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

        private void Start()
        {
            TaskManager.Instance.currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
        }

        public void NextGoKart()
        {
            // TODO: Drive current GoKart out of Garage.
            
            // TODO: Kill current GoKart.
            
            // TODO: Create new GoKart.
            
            // TODO: Replace old GoKart References with new GoKart.
            
            // TODO: Update UI.
        }
    }
}
