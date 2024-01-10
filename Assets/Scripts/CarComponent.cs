using UnityEngine;
using UnityEngine.Serialization;

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

    public CarPart carPart;
    public Status status;

    [FormerlySerializedAs("toolToRepair")] public ToolStation toolStationToRepair;
}
