using System.Collections.Generic;
using Karts;

namespace Task
{
    public class TaskManager
    {
        private static TaskManager _instance;

        public static TaskManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskManager();
                }

                return _instance;
            }
        }

        public readonly List<CarComponent> brokenParts = new List<CarComponent>();
        public readonly List<CarComponent> damagedParts = new List<CarComponent>();
        
        public delegate void GoKartFinished();
        public event GoKartFinished OnGokartFinished;

        public GoKart currentGoKart;

        private TaskManager() { }

        public void AddBrokenPart(CarComponent brokenPart)
        {
            brokenParts.Add(brokenPart);
        }

        public void AddDamagedPart(CarComponent damagedPart)
        {
            damagedParts.Add(damagedPart);
        }

        public void RemoveBrokenPart(CarComponent part)    // Component replaced.
        {
            brokenParts.Remove(part);
        }

        public void RemoveDamagedPart(CarComponent part)    // Component repaired.
        {
            damagedParts.Remove(part);
            CheckIfCurrentGoKartIsFinished();
        }

        public void AddedCarComponent()                     // Component added.
        {
            CheckIfCurrentGoKartIsFinished();
        }

        private void CheckIfCurrentGoKartIsFinished()
        {
            if (currentGoKart.carComponents.Length == currentGoKart.intactParts.Count)
            {
                OnGokartFinished?.Invoke();
            }
        }
    }
}
