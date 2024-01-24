using UnityEngine;
using Task;
using TMPro;
using UI;

public class MoneyEconomy : MonoBehaviour
{
    [Header("UI Top Bar")]
    public TextMeshProUGUI moneyUI;
    public Timer timer;

    private const string moneyCounterUILabel = "Money: ";

    private const int repairLoan = 10;
    private const int replaceLoan = 30;
    private const int finishedGoKartLoan = 50;

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
        Time.timeScale = 0;
        
        UPDATE_EconomyOverview();
        
        EconomyOverviewUI.SetActive(true);
    }

    public void NextDay()
    {
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

        currentMoneyAmount -= rentAmount;
        
        moneyAmountLastDayUI.text = moneyAmountLastDay.ToString();
        moneyAmountThroughRepairingUI.text = moneyAmountThroughRepairing.ToString();
        moneyAmountThroughReplacingUI.text = moneyAmountThroughReplacing.ToString();
        moneyAmountThroughFinishingGoKartsUI.text = moneyAmountThroughFinishingGoKarts.ToString();
        rentAmountUI.text = rentAmount.ToString();
        currentMoneyAmountUI.text = currentMoneyAmount.ToString();
    }
}
