using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UI;

namespace Task
{
    public class MoneyEconomy : MonoBehaviour
    {
        private AudioSource audioSource;
    
        [Header("Audio CLips")]
        public AudioClip economySFX;
        public AudioClip gameOverSFX;
    
        [Space(20)]
        [Header("UI Top Bar")]
        public TextMeshProUGUI moneyUI;
        public Timer timer;

        private const string moneyCounterUILabel = "Money: ";

        private const int repairLoan = 10;
        private const int replaceLoan = 30;
        private const int finishedGoKartLoan = 50;
        private const int raisingRentAmount = 30;

        private int moneyAmountLastDay;
        private int moneyAmountThroughRepairing;
        private int moneyAmountThroughReplacing;
        private int moneyAmountThroughFinishingGoKarts;

        [SerializeField] private int rentAmount = 50;
    
        private int currentMoneyAmount;

        [Space(20)]
        [Header("End Of The Day Economy Overview")]
        public GameObject EconomyOverviewUI;
        [Space(5)]
        public TextMeshProUGUI moneyAmountLastDayUI;
        public TextMeshProUGUI moneyAmountThroughRepairingUI;
        public TextMeshProUGUI moneyAmountThroughReplacingUI;
        public TextMeshProUGUI moneyAmountThroughFinishingGoKartsUI;
        public TextMeshProUGUI rentAmountUI;
        public TextMeshProUGUI currentMoneyAmountUI;

        private int repairedCarComponentsAmount;
        private int replacedCarComponentsAmount;
        private int finishedGoKartsAmount;

        [Space(20)] [Header("Game Over Screen")]
        public GameObject GameOverScreen;
    
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = economySFX;
            
            TaskManager.Instance.OnGoKartFinished += TaskManager_OnGoKartFinished;
            UpdateMoneyUI();
        }

        public void RemoveDamagedPart()
        {
            currentMoneyAmount += repairLoan;
            UpdateMoneyUI();
            PlayMoneyEconomySFX();
            repairedCarComponentsAmount++;
        }

        public void RemoveBrokenPart()
        {
            currentMoneyAmount += replaceLoan;
            UpdateMoneyUI();
            PlayMoneyEconomySFX();
            replacedCarComponentsAmount++;
        }

        private void TaskManager_OnGoKartFinished()
        {
            currentMoneyAmount += finishedGoKartLoan;
            UpdateMoneyUI();
            PlayMoneyEconomySFX();
            finishedGoKartsAmount++;
        }

        private void UpdateMoneyUI()
        {
            moneyUI.text = moneyCounterUILabel + currentMoneyAmount;
        }

        private void PlayMoneyEconomySFX()
        {
            audioSource.clip = economySFX;
            audioSource.Play();
        }

        public void EndOfTheDay()
        {
            UPDATE_EconomyOverview();
        
            EconomyOverviewUI.SetActive(true);
        
            audioSource.Play();
        
            Time.timeScale = 0;
        }

        public void NextDay()
        {
            if (currentMoneyAmount < 0)
            {
                EndGame();
                return;
            }
        
            EconomyOverviewUI.SetActive(false);
            float decreasingTime = (timer.dayCounter.CurrentDay - 1) * 20f;
            
            if (timer.startTimer - decreasingTime < 20f)
                timer.remainingTime = 20f;
            else
                timer.remainingTime = timer.startTimer - decreasingTime;
            
            timer.InternalIsEmergent = false;
            timer.timerUI.color = Color.white;
            timer.clockIcon.color = Color.white;

            moneyAmountLastDay = currentMoneyAmount;
            repairedCarComponentsAmount = 0;
            replacedCarComponentsAmount = 0;
            finishedGoKartsAmount = 0;

            Time.timeScale = 1;
        }

        private void UPDATE_EconomyOverview()
        {
            moneyAmountThroughRepairing = repairedCarComponentsAmount * repairLoan;
            moneyAmountThroughReplacing = replacedCarComponentsAmount * replaceLoan;
            moneyAmountThroughFinishingGoKarts = finishedGoKartsAmount * finishedGoKartLoan;

            int currentRentAmount = (timer.dayCounter.CurrentDay - 2) * raisingRentAmount + rentAmount;
            currentMoneyAmount -= currentRentAmount;
        
            moneyAmountLastDayUI.text = moneyAmountLastDay.ToString();
            moneyAmountThroughRepairingUI.text = moneyAmountThroughRepairing.ToString();
            moneyAmountThroughReplacingUI.text = moneyAmountThroughReplacing.ToString();
            moneyAmountThroughFinishingGoKartsUI.text = moneyAmountThroughFinishingGoKarts.ToString();
            rentAmountUI.text = "-" + currentRentAmount;
            currentMoneyAmountUI.text = currentMoneyAmount.ToString();

            moneyUI.text = moneyCounterUILabel + currentMoneyAmount;
        }

        private void EndGame()
        {
            Time.timeScale = 1;
        
            EconomyOverviewUI.SetActive(false);
        
            GameOverScreen.SetActive(true);
            GameOverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "Days Persevered: " + (timer.dayCounter.CurrentDay - 1);

            Destroy(TaskManager.Instance.soundManager.GetComponent<SoundManager>());
            Destroy(TaskManager.Instance.soundManager.GetComponent<AudioSource>());
        
            audioSource.clip = gameOverSFX;
            audioSource.Play();
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
