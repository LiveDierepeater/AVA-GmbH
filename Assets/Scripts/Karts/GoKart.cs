using System.Collections.Generic;
using Task;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Karts
{
    public class GoKart : MonoBehaviour
    {
        public CarComponent[] carComponents;

        public List<CarComponent> brokenParts;
        public List<CarComponent> damagedParts;
        public List<CarComponent> intactParts;

        public bool debugCarComponents;
        public bool debugCarComponentsUI;

        public TextMeshProUGUI carComponentsUI;
    
        private void Start()
        {
            RollCarComponentsStatus();
            ListInOrderOfStatus();
            AddCarComponentsToTaskManager();

            DEBUG_CarComponents();
            DEBUG_CarComponentsUI();
        }

        private void AddCarComponentsToTaskManager()
        {
            foreach (CarComponent brokenPart in brokenParts)
            {
                TaskManager.Instance.AddBrokenPart(brokenPart);
            }
            foreach (CarComponent damagedPart in damagedParts)
            {
                TaskManager.Instance.AddDamagedPart(damagedPart);
            }
        }

        private void RollCarComponentsStatus()
        {
            foreach (CarComponent carComponent in carComponents)
            {
                int newStatus = Random.Range(0, 3);

                carComponent.status = newStatus switch
                {
                    0 => CarComponent.Status.Broken,
                    1 => CarComponent.Status.Damaged,
                    2 => CarComponent.Status.Intact,
                    _ => carComponent.status
                };
            }
        }

        private void ListInOrderOfStatus()
        {
            foreach (CarComponent part in carComponents)
            {
                switch (part.status)
                {
                    case CarComponent.Status.Broken:
                        brokenParts.Add(part);
                        break;
                
                    case CarComponent.Status.Damaged:
                        damagedParts.Add(part);
                        break;
                
                    case CarComponent.Status.Intact:
                        break;
                
                    default:
                        intactParts.Add(part);
                        break;
                }
            }
        }

        private void DEBUG_CarComponents()
        {
            if (!debugCarComponents) return;
        
            foreach (CarComponent carComponent in brokenParts)
            {
                Debug.Log(carComponent.carPart + ", " + carComponent.status);
            }
            foreach (CarComponent carComponent in damagedParts)
            {
                Debug.Log(carComponent.carPart + ", " + carComponent.status);
            }
            foreach (CarComponent carComponent in intactParts)
            {
                Debug.Log(carComponent.carPart + ", " + carComponent.status);
            }
        }

        private void DEBUG_CarComponentsUI()
        {
            if (!debugCarComponentsUI) return;

            carComponentsUI.text = "";
            
            foreach (CarComponent carComponent in carComponents)
            {
                if (carComponent.status == CarComponent.Status.Damaged)
                {
                    carComponentsUI.text += "\n";
                    carComponentsUI.text += carComponent.name + ", Tool: " + carComponent.toolToRepair.name;
                }
            }
        }
    }
}
