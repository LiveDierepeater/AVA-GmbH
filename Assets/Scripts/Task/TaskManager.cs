using System.Collections.Generic;
using System.Security.Principal;
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
        }

        public void AddedCarComponent()                     // Component added.
        {
            
        }

        public void GoKartFinished()
        {
            
        }
    }
}
