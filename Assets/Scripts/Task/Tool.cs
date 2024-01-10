using UnityEngine;

namespace Task
{
    public class Tool : MonoBehaviour
    {
        public enum Type
        {
            Wrench,
            Rag,
            Oil,
            Kable,
            Screwdriver,
            Gasoline
        }

        public Type toolType;
    }
}
