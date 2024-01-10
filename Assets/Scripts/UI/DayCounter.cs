using TMPro;
using UnityEngine;

namespace UI
{
    public class DayCounter : MonoBehaviour
    {
        private TextMeshProUGUI dayCounterUI;
    
        private int internalCurrentDay = 1;
        private const string dayLABEL = "Day: ";

        public int CurrentDay
        {
            get => internalCurrentDay;
            set
            {
                internalCurrentDay = value;
                UpdateUI();
            }
        }

        private void Awake()
        {
            dayCounterUI = GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }

        private void UpdateUI()
        {
            dayCounterUI.text = dayLABEL + CurrentDay;
        }
    }
}
