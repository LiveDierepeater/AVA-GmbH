using System.Collections.Generic;

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

    private TaskManager() { }

    public void AddBrokenPart(CarComponent brokenPart)
    {
        brokenParts.Add(brokenPart);
    }

    public void AddDamagedPart(CarComponent damagedPart)
    {
        damagedParts.Add(damagedPart);
    }

    public void RemoveBrokenPart(CarComponent part)
    {
        brokenParts.Remove(part);
    }

    public void RemoveDamagedPart(CarComponent part)
    {
        damagedParts.Remove(part);
    }
}
