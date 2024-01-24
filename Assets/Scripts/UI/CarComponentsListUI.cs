using UnityEngine;
using System.Collections.Generic;
using Characters;
using Karts;
using Task;

namespace UI
{
    public class CarComponentsListUI : MonoBehaviour
    {
        public enum Status
        {
            Broken,
            Damaged
        }

        public Status status;

        public GameObject brokenCarComponentUIPrefab;
        public GameObject damagedCarComponentUIPrefab;
    
        private GoKart currentGoKart;

        private List<CarComponentUI> damagedCarComponentsUIList = new List<CarComponentUI>();
        private List<CarComponentUI> brokenCarComponentsUIList = new List<CarComponentUI>();

        private string brokenCarComponentUIInstruction_01 = "Remove Component";
        private string brokenCarComponentUIInstruction_02 = "Add new Component";

        private void Awake()
        {
            Invoke(nameof(InitializeLists), 0.1f);
        }

        private void NewGoKartReference()
        {
            currentGoKart = TaskManager.Instance.currentGoKart;
        }

        private void Start()
        {
            // Initializing GameManager Reference for OnNextGoKart Event.
            GameManager gameManager = TaskManager.Instance.gameManager;
            gameManager.OnNextGoKart += NewGoKartReference;
            
            // Initializing GoKart Reference.
            currentGoKart = TaskManager.Instance.currentGoKart;

            // Red Coloring.
            foreach (CarComponentUI ui in damagedCarComponentsUIList)
            {
                ui.carComponentInstructionUGUI.text = "Get Tool";
                ui.carComponentInstructionField.color = new Color(0.8f, 0.05f, 0.05f, 1);    
            }
        }

        public void RemoveBrokenCarComponentFromUIList(CarComponent fixedCarComponent)
        {
            CarComponentUI carComponentUIToDelete = null;

            foreach (CarComponentUI brokenCarComponentUI in brokenCarComponentsUIList)
            {
                if (fixedCarComponent.name == brokenCarComponentUI.carComponentNameReference)
                {
                    carComponentUIToDelete = brokenCarComponentUI;
                }
            }
            
            if (carComponentUIToDelete is null) return;

            brokenCarComponentsUIList.Remove(carComponentUIToDelete);
            Destroy(carComponentUIToDelete.gameObject);
        }

        public void RemoveDamagedCarComponentsFromUIList(CarComponent fixedCarComponent)
        {
            CarComponentUI carComponentUIToDelete = null;
            
            foreach (CarComponentUI damagedCarComponentUI in damagedCarComponentsUIList)
            {
                if (fixedCarComponent.name + "(Clone)" == damagedCarComponentUI.carComponentNameReference)
                {
                    carComponentUIToDelete = damagedCarComponentUI;
                }
            }
            
            if (carComponentUIToDelete is null) return;

            damagedCarComponentsUIList.Remove(carComponentUIToDelete);
            Destroy(carComponentUIToDelete.gameObject);
        }

        public void UPDATE_LooseCarComponentUI(CarComponent looseCarComponent)
        {
            foreach (CarComponentUI brokenCarComponentUI in brokenCarComponentsUIList)
            {
                
                if (looseCarComponent.name + "(Clone)" == brokenCarComponentUI.carComponentNameReference)
                {
                    brokenCarComponentUI.carComponentInstructionUGUI.text = brokenCarComponentUIInstruction_02;
                    
                    // Set UI color.
                    brokenCarComponentUI.carComponentInstructionField.color = Color.yellow;
                }
            }
        }

        public void InitializeLists()
        {
            if (status == Status.Broken)
                foreach (CarComponent brokenPart in currentGoKart.brokenParts)
                {
                    GameObject newCarComponentUIInstance = Instantiate(brokenCarComponentUIPrefab, transform);
                    CarComponentUI newCarComponentUI = newCarComponentUIInstance.GetComponent<CarComponentUI>();

                    newCarComponentUI.carComponentIconImage.sprite = brokenPart.carComponentUISprite;
                    newCarComponentUI.carComponentInstructionUGUI.text = brokenCarComponentUIInstruction_01;
                    newCarComponentUI.carComponentNameReference = brokenPart.name + "(Clone)";
                    brokenCarComponentsUIList.Add(newCarComponentUI);
                    
                    // Set UI color.
                    newCarComponentUI.carComponentInstructionField.color = new Color(0.8f, 0.05f, 0.05f, 1);
                }
            else if (status == Status.Damaged)
                foreach (CarComponent damagedPart in currentGoKart.damagedParts)
                {
                    GameObject newCarComponentUIInstance = Instantiate(damagedCarComponentUIPrefab, transform);
                    CarComponentUI newCarComponentUI = newCarComponentUIInstance.GetComponent<CarComponentUI>();

                    newCarComponentUI.carComponentIconImage.sprite = damagedPart.carComponentUISprite;
                    newCarComponentUI.toolToRepairIconImage.sprite = damagedPart.toolToRepair.toolUISprite;
                    newCarComponentUI.carComponentNameReference = damagedPart.name + "(Clone)";
                    damagedCarComponentsUIList.Add(newCarComponentUI);
                    
                    // Set UI color.
                    newCarComponentUI.carComponentInstructionField.color = new Color(0.8f, 0.05f, 0.05f, 1);
                }
        }

        private void Update()
        {
            if (status == Status.Broken) return;

            foreach (CarComponentUI ui in damagedCarComponentsUIList)
            {
                ui.carComponentInstructionUGUI.text = "Get Tool";
                ui.carComponentInstructionField.color = new Color(0.8f, 0.05f, 0.05f, 1);
                
                foreach (SelectableUnit unit in SelectionManager.Instance.AvailableUnits)
                {
                    if (unit.toolSlot.childCount == 0) continue;

                    if (ui.toolToRepairIconImage.sprite.name ==
                        unit.toolSlot.GetChild(0).GetComponent<Tool>().toolUISprite.name)
                    {
                        ui.carComponentInstructionUGUI.text = "Repair Kart";
                        ui.carComponentInstructionField.color = Color.yellow;
                    }
                }
            }
        }
    }
}
