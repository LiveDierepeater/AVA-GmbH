using UnityEngine;
using Task;
using TMPro;
using UI;
using UnityEngine.SceneManagement;

public class MoneyEconomy : MonoBehaviour
{
    [Header("UI Top Bar")]
    public TextMeshProUGUI moneyUI;
    public Timer timer;

    private const string moneyCounterUILabel = "Money: ";

    private const int repairLoan = 10;
    private const int replaceLoan = 30;
    private const int finishedGoKartLoan = 50;
    private const int raisingRentAmount = 20;

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

    [Space(20)] [Header("End Of The Day Economy Overview")]
    public GameObject GameOverScreen;
    
    private void Awake()
    {
        TaskManager.Instance.OnGoKartFinished += TaskManager_OnGoKartFinished;
        UpdateMoneyUI();
    }

    public void RemoveDamagedPart()
    {
        currentMoneyAmount += repairLoan;
        UpdateMoneyUI();
        repairedCarComponentsAmount++;
    }

    public void RemoveBrokenPart()
    {
        currentMoneyAmount += replaceLoan;
        UpdateMoneyUI();
        replacedCarComponentsAmount++;
    }

    private void TaskManager_OnGoKartFinished()
    {
        currentMoneyAmount += finishedGoKartLoan;
        UpdateMoneyUI();
        finishedGoKartsAmount++;
    }

    private void UpdateMoneyUI()
    {
        moneyUI.text = moneyCounterUILabel + currentMoneyAmount;
    }

    public void EndOfTheDay()
    {
        UPDATE_EconomyOverview();
        
        EconomyOverviewUI.SetActive(true);
        
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
        timer.remainingTime = timer.startTimer;

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
        rentAmountUI.text = currentRentAmount.ToString();
        currentMoneyAmountUI.text = currentMoneyAmount.ToString();

        moneyUI.text = moneyCounterUILabel + currentMoneyAmount;
    }

    private void EndGame()
    {
        Time.timeScale = 1;
        
        EconomyOverviewUI.SetActive(false);
        
        GameOverScreen.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}