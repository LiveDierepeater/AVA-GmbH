using System.Collections.Generic;

namespace Characters
{
    public class SelectionManager
    {
        private static SelectionManager _instance;

        public static SelectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SelectionManager();
                }

                return _instance;
            }
        }

        public readonly HashSet<SelectableUnit> SelectedUnits = new HashSet<SelectableUnit>();
        public readonly List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();

        private SelectionManager() { }

        public void Select(SelectableUnit unit)
        {
            unit.OnSelected();
            SelectedUnits.Add(unit);
        }

        public void Deselect(SelectableUnit unit)
        {
            unit.OnDeselected();
            SelectedUnits.Remove(unit);
        }

        public void DeselectAll()
        {
            foreach (SelectableUnit unit in SelectedUnits)
            {
                unit.OnDeselected();
            }
        
            SelectedUnits.Clear();
        }

        public bool IsSelected(SelectableUnit unit)
        {
            return SelectedUnits.Contains(unit);
        }
    }
}
