using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public DayCounter dayCounter;
    
        private TextMeshProUGUI timerUI;
        private Image clockIcon;
    
        public float remainingTime;

        public Color emergencyColor = Color.red;

        private void Awake()
        {
            timerUI = GetComponent<TextMeshProUGUI>();
            clockIcon = GetComponentInChildren<Image>();
        }

        private void Update()
        {
            if (remainingTime < 11)
            {
                timerUI.color = emergencyColor;
                clockIcon.color = emergencyColor;
            }
        
            if (remainingTime > 0)
                remainingTime -= Time.deltaTime;
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                dayCounter.CurrentDay++;
                return;
            }
        
            float minutes = Mathf.FloorToInt(remainingTime / 60);
            float seconds = Mathf.FloorToInt(remainingTime % 60);
            timerUI.text = $"{minutes:00}:{seconds:00}";
        }
    }
}