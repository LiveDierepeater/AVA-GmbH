using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class CarComponentUI : MonoBehaviour
    {
        public Image carComponentIconImage;
    
        public string carComponentNameReference;
    
        [Header("UI Left Side")] public TextMeshProUGUI carComponentInstructionUGUI;
        
        public Image carComponentInstructionField;

        [Header("UI Right Side")] public Image toolToRepairIconImage;
    }
}
