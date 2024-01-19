using UnityEngine;
using UnityEngine.UI;
using Characters;
using TMPro;

public class UnitUIController : MonoBehaviour
{
    public SelectableUnit unit;
    
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitState;
    
    public Image unitAvatarIcon;
    public Image equippedItemIcon;

    private void Awake()
    {
        unit.SetUnitUIController(this);
        InitializeUnitAvatar();
    }

    private void InitializeUnitAvatar()
    {
        unitName.text = unit.name;
        unitAvatarIcon.sprite = unit.unitsUIAvatarSprite;
    }

    public void UPDATE_UnitUI()
    {
        unitState.text = unit.currentState.ToString();
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
}
