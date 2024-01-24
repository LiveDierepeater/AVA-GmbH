using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public DayCounter dayCounter;

        [Header("AudioClip")]
        public AudioClip emergencySFX;
        
        private TextMeshProUGUI timerUI;
        private Image clockIcon;
        private AudioSource audioSource;

        public float startTimer = 60f;
        
        public float remainingTime;
        public float alertTime = 30;

        public Color emergencyColor = Color.red;

        private bool InternalIsEmergent;
        
        private bool IsEmergent
        {
            get => InternalIsEmergent;
            set
            {
                InternalIsEmergent = value;
                PlayAlarmSound();
            }
        }

        private void Awake()
        {
            timerUI = GetComponent<TextMeshProUGUI>();
            clockIcon = GetComponentInChildren<Image>();
            
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = emergencySFX;
        }

        private void Update()
        {
            if (remainingTime < alertTime + 1)
            {
                timerUI.color = emergencyColor;
                clockIcon.color = emergencyColor;
                
                
                // Play Emergent Sound
                if (!IsEmergent)
                    IsEmergent = true;
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

        private void PlayAlarmSound()
        {
            audioSource.Play();
        }
    }
}
