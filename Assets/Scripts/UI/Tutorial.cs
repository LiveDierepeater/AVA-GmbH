using System.Collections.Generic;
using UnityEngine;
using Characters;
using Karts;
using Task;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    private const string label_01 = "Das sind deine Arbeiter.\n\nDu kannst einen oder mehrere Arbeiter mit der \"linken Maustaste\" auswählen.";
    private const string label_02 = "Mit der \"rechten Maustaste\" kannst du deine Arbeiter bewegen und \"Werkzeuge\" ausrüsten lassen.";
    private const string label_03 = "Ein Arbeiter mit dem passenden \"Werkzeug\" kann das Go-Kart reparieren.";
    private const string label_04 = "Das ist dein erster Kunde.\n\nWähle das passende Werkzeug aus und versuche das Go-Kart zu reparieren.";
    private const string label_05 = "Nun scheint es, als würden manche Teile ersetzt werden müssen.";
    private const string label_06 = "Schicke einen Arbeiter ohne \"Werkzeuge\" auf das Go-Kart los, um kaputte \"Teile\" des Go-Karts auszubauen.";
    private const string label_07 = "Werfe das kaputte Teil in den Müll.\n\nDeine Arbeiter werden mit der Taste \"S\" einen Gegenstand dort fallen lassen, wo sich deine Maus befindet.";
    private const string label_08 = "Hole das passende \"Teil\" aus dem Lager.\n\nDen Raum kannst du mit \"D\" und \"A\" und auch mit den \"Pfeiltasten\" wechseln.";
    private const string label_09 = "Baue das neue \"Teil\" ein.";
    private const string label_10 = "Oben rechts siehst du dein verdientes Geld.\n\n Achte darauf, dass du genug Geld für deine Miete hast.";
    private const string label_11 = "Nun solltest du alles gelernt haben.\n\nViel Spaß in der Werkstatt!";

    private readonly List<string> labels = new List<string>();

    public int currentTip;
    
    private void Awake()
    {
        InitializeLabels();
    }

    private void InitializeLabels()
    {
        labels.Add(label_01);
        labels.Add(label_02);
        labels.Add(label_03);
        labels.Add(label_04);
        labels.Add(label_05);
        labels.Add(label_06);
        labels.Add(label_07);
        labels.Add(label_08);
        labels.Add(label_09);
        labels.Add(label_10);
        labels.Add(label_11);
    }

    public void NextTip()
    {
        currentTip++;
        UpdateTipUI();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void UpdateTipUI()
    {
        // TODO: Load Main Scene, when Tutorial is Done!
        if (currentTip == labels.Count)
        {
            SceneManager.LoadScene(0);
            return;
        }
        
        textBox.text = labels.ToArray()[currentTip];
    }

    public void ShowTip()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentTip == 1)
            if (SelectionManager.Instance.SelectedUnits.Count > 0)
                ShowTip();

        if (currentTip == 2)
            if (Input.GetMouseButtonDown(1))
                if (SelectionManager.Instance.SelectedUnits.Count > 0)
                    ShowTip();
        
        if (currentTip == 3)
            ShowTip();
        
        if (currentTip == 4)
            if (TaskManager.Instance.currentGoKart.goKartStatus == GoKart.Status.DrivingOut)
                ShowTip();
        
        if (currentTip == 5)
            ShowTip();
        
        if (currentTip == 6)
            foreach (SelectableUnit unit in SelectionManager.Instance.SelectedUnits)
                if (unit.componentSlot.childCount > 0 &&
                    unit.componentSlot.GetChild(0).GetComponent<CarComponent>().status == CarComponent.Status.Broken)
                    ShowTip();
        
        if (currentTip == 7)
            if (Input.GetKeyDown(KeyCode.S))
                if (SelectionManager.Instance.SelectedUnits.Count > 0)
                    ShowTip();
        
        if (currentTip == 8)
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                ShowTip();
        
        if (currentTip == 9)
            if (TaskManager.Instance.currentGoKart.goKartStatus == GoKart.Status.DrivingOut)
                ShowTip();
        
        if (currentTip == 10)
            ShowTip();
    }
}
