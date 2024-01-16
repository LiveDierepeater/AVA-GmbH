using UnityEngine;
using System.Collections.Generic;
using Karts;
using TMPro;

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

        public GameObject carComponentTextUIPrefab;
    
        private GoKart currentGoKart;

        private List<GameObject> carComponentsUIList = new List<GameObject>();

        private void Awake()
        {
            currentGoKart = GameObject.Find("GoKart").GetComponent<GoKart>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (status == Status.Broken)
                    foreach (CarComponent brokenPart in currentGoKart.brokenParts)
                    {
                        GameObject newText = Instantiate(carComponentTextUIPrefab, transform);
                        newText.GetComponentInChildren<TextMeshProUGUI>().text = brokenPart.carPartType.ToString();
                        carComponentsUIList.Add(newText);
                    }
                else if (status == Status.Damaged)
                    foreach (CarComponent damagedPart in currentGoKart.damagedParts)
                    {
                        GameObject newText = Instantiate(carComponentTextUIPrefab, transform);
                        newText.GetComponentInChildren<TextMeshProUGUI>().text = damagedPart.carPartType.ToString();
                        carComponentsUIList.Add(newText);
                    }
            }
        }

        public void UDPATE_UILists()
        {
            GameObject carComponentUIToDelete = null;
        
            foreach (CarComponent intactPart in currentGoKart.intactParts)
            {
                foreach (GameObject ui in carComponentsUIList)
                {
                    if (intactPart.carPartType.ToString() == ui.GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        carComponentUIToDelete = ui;
                    }
                }
            }
        
            if (carComponentUIToDelete is null) return;
        
            carComponentsUIList.Remove(carComponentUIToDelete);
            Destroy(carComponentUIToDelete);
        }
    }
}
