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

        private string moneyCounterUILabel = "Money: ";
        private int currentMoneyAmount = 0;

        public void UPDATE_RemoveDamagedPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveDamagedPart(carComponent);
            carComponentsRightListUI.UDPATE_UILists();
            currentMoneyAmount += 10;
            moneyCounterUI.text = moneyCounterUILabel + currentMoneyAmount;
        }

        public void UPDATE_RemoveBrokenPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveBrokenPart(carComponent);
            carComponentsLeftListUI.UDPATE_UILists();
            currentMoneyAmount += 10;
            moneyCounterUI.text = moneyCounterUILabel + currentMoneyAmount;
        }
    }
}
