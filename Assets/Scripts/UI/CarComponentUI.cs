using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarComponentUI : MonoBehaviour
{
    public Image carComponentIconImage;
    
    public string carComponentNameReference;
    
    [Header("UI Left Side")] public TextMeshProUGUI carComponentInstructionUGUI;

    [Header("UI Right Side")] public Image toolToRepairIconImage;
}
