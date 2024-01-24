using System;
using Task;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DayCounter : MonoBehaviour
    {
        private TextMeshProUGUI dayCounterUI;
    
        private int internalCurrentDay = 1;
        private const string dayLABEL = "Day: ";

        private MoneyEconomy moneyEconomy;

        public int CurrentDay
        {
            get => internalCurrentDay;
            set
            {
                internalCurrentDay = value;
                UpdateUI();
                
                moneyEconomy.EndOfTheDay();
            }
        }

        private void Awake()
        {
            dayCounterUI = GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }

        private void Start()
        {
            moneyEconomy = TaskManager.Instance.gameManager.gameObject.GetComponent<MoneyEconomy>();
        }

        private void UpdateUI()
        {
            dayCounterUI.text = dayLABEL + CurrentDay;
        }
    }
}
