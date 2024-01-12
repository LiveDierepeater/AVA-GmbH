using Task;
using UnityEngine;
using UnityEngine.Serialization;

namespace Karts
{
    public class CarComponent : MonoBehaviour
    {
        public enum CarPart
        {
            Airfilter,
            Wheels,
            Battery,
            Brakes,
            Motorchain,
            Motorblock,
            Oil,
            Exhaust,
            Tankpump,
            Body,
            Dampers,
            Seat,
            Seatbelt,
            Axe,
            Gasoline,
            Pedals
        }
    
        public enum Status
        {
            Broken,
            Damaged,
            Intact
        }

        [FormerlySerializedAs("carPart")] public CarPart carPartType;
        public Status status;

        public Tool toolToRepair;
    }
}
