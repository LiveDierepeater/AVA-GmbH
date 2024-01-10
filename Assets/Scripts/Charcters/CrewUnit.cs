using System;
using UnityEngine;

namespace Charcters
{
    public class CrewUnit : MonoBehaviour
    {
        private Transform toolSlot;
        
        private void Awake()
        {
            toolSlot = transform.Find("Tool Slot");

            
        }
    }
}
