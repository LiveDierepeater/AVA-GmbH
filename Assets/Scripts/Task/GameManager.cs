using UnityEngine;
using Karts;
using UI;

namespace Task
{
    public class GameManager : MonoBehaviour
    {
        public CarComponentsListUI carComponentsLeftListUI;
        public CarComponentsListUI carComponentsRightListUI;

        public void UPDATE_RemoveDamagedPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveDamagedPart(carComponent);
            carComponentsRightListUI.UDPATE_UILists();
        }

        public void UPDATE_RemoveBrokenPart(CarComponent carComponent)
        {
            TaskManager.Instance.RemoveBrokenPart(carComponent);
            carComponentsLeftListUI.UDPATE_UILists();
        }
    }
}
