using UnityEngine;
using UnityEngine.Serialization;
using Task;

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
            GasolineTank,
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
