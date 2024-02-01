using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Characters;
using Karts;
using Task;
using TMPro;

namespace UI
{
    public class Tutorial : MonoBehaviour
    {
        public TextMeshProUGUI textBox;

        private const string label_01 = "These are your workers!\n\nSelect one or more workers with your \"left mouse button\".\n\n Also you can select them by clicking on their UI at the bottom.\n\n Or with the \"1, 2 and 3\" keys.";
        private const string label_02 = "With the \"right mouse button\", you can move your workers and equip them with \"tools\".";
        private const string label_03 = "A worker with the fitting \"tool\" can go and repair the Go-Kart.";
        private const string label_04 = "There is your first customer!\n\nSelect the fitting tool and try repairing the Go-Kart.\n\nYou can put an equipped \"tool\" back onto its proper Station.";
        private const string label_05 = "Seems like you need to replace some Kart parts now.";
        private const string label_06 = "Send your worker without any \"tools\" to remove broken parts from the Go-Kart.\n\nYou can Zoom with your \"mouse wheel\".";
        private const string label_07 = "Go ahead and throw away the broken part.\n\nYour worker will drop the part where your cursor is hovering over when you press \"S\"";
        private const string label_08 = "Fetch the new fitting \"part\" from the warehouse\n\nYou can change to the warehouse and back by pressing \"D\" and \"A\".\n\nYou can also use your arrow keys.";
        private const string label_09 = "Attach the new fitting \"part\".";
        private const string label_10 = "Top right you can see your earned cash.\n\n Don't forget to meet rent!";
        private const string label_11 = "Good job learning how to play.\n\nNow, Good luck with your new Go-Kart workshop!";

        private readonly List<string> labels = new List<string>();

        private int currentTip;
    
        private void Awake()
        {
            InitializeLabels();
            textBox.text = label_01;
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
}
