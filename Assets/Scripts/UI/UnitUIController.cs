using UnityEngine;
using UnityEngine.UI;
using Characters;
using TMPro;

namespace UI
{
    public class UnitUIController : MonoBehaviour
    {
        public SelectableUnit unit;
    
        public TextMeshProUGUI unitName;
        public TextMeshProUGUI unitState;
    
        public Image unitAvatarIcon;
        public Image equippedItemIcon;

        [Header("Working Animation")] 
        public Image unitAnimationUI;

        private Image unitUIBorder;

        public enum Unit
        {
            Bunny,
            Elephant,
            Horse
        }

        public Unit unitType;

        private void Awake()
        {
            GetUnitReference();
            unit.SetUnitUIController(this);
            InitializeUnitAvatar();

            unitUIBorder = GetComponent<Image>();
        }

        private void GetUnitReference()
        {
            GameObject units = GameObject.Find("Units");
            
            foreach (SelectableUnit unitToInitialize in units.GetComponentsInChildren<SelectableUnit>())
            {
                if (unitToInitialize.name == unitType.ToString())
                    unit = unitToInitialize;
            }
        }

        private void InitializeUnitAvatar()
        {
            unitName.text = unit.name;
            unitAvatarIcon.sprite = unit.unitsUIAvatarSprite;
        }

        public void UPDATE_UnitUI()
        {
            if (unit.currentState == SelectableUnit.States.Idle)
                unitState.text = "";
            
            if (unit.currentState == SelectableUnit.States.GetTool)
                unitState.text = "Get Tool";
            
            if (unit.currentState == SelectableUnit.States.GetCarComponent)
                unitState.text = "Get Part";
            
            if (unit.currentState == SelectableUnit.States.AddCarComponent ||
                unit.currentState == SelectableUnit.States.RemoveCarComponent ||
                unit.currentState == SelectableUnit.States.RepairKart)
                unitState.text = "Working";
            
            if (unit.currentState == SelectableUnit.States.MoveToDestination ||
                unit.currentState == SelectableUnit.States.DropOffItem)
                unitState.text = "Running";
            
            equippedItemIcon.sprite = unit.GetEquippedItemUISprite();
        
            if (equippedItemIcon.sprite is null) equippedItemIcon.color = Color.clear;
            else equippedItemIcon.color = Color.white;
        }

        public void ClearItemUISprite()
        {
            equippedItemIcon.sprite = null;
            equippedItemIcon.color = Color.clear;
        
            Invoke(nameof(UPDATE_UnitUI), 0.03f);
        }

        public void HideWorkingAnimationUI()
        {
            if (unitAnimationUI.color == Color.white)
                unitAnimationUI.color = Color.clear;
        }

        public void ShowWorkingAnimationUI()
        {
            if (unitAnimationUI.color == Color.clear)
                unitAnimationUI.color = Color.white;
        }
    }
}
