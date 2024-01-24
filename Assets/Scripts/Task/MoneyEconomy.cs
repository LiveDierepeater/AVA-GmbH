using UnityEngine;
using Task;
using TMPro;

public class MoneyEconomy : MonoBehaviour
{
    public TextMeshProUGUI moneyUI;
    
    private readonly string moneyCounterUILabel = "Money: ";

    private const int repairLoan = 10;
    private const int replaceLoan = 30;
    private const int finishedGoKartLoan = 50;

    private int moneyAmountLastDay;
    private int moneyAmountThroughRepairing;
    private int moneyAmountThroughReplacing;
    private int moneyAmountThroughFinishingGoKarts;

    [SerializeField] private int rentAmount = 50;
    
    private int currentMoneyAmount;
    
    private void Awake()
    {
        TaskManager.Instance.OnGoKartFinished += TaskManager_OnGoKartFinished;
        UpdateMoneyUI();
    }

    public void RemoveDamagedPart()
    {
        currentMoneyAmount += repairLoan;
        UpdateMoneyUI();
    }

    public void RemoveBrokenPart()
    {
        currentMoneyAmount += replaceLoan;
        UpdateMoneyUI();
    }

    private void TaskManager_OnGoKartFinished()
    {
        currentMoneyAmount += finishedGoKartLoan;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        moneyUI.text = moneyCounterUILabel + currentMoneyAmount;
    }
}
