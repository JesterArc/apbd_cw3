namespace Task3;

public class ContainerShip
{
    public List<Container> _containers{ get; private set; }
    private readonly double _maxSpeed;
    private readonly int _maxContainerCount;
    private readonly double _maxCarryingCapacity;
    private double _currentCarryingCapacity;

    public ContainerShip(double maxSpeed, int maxContainerCount, double maxCarryingCapacity)
    {
        _maxSpeed = maxSpeed;
        _containers = [];
        _maxContainerCount = maxContainerCount;
        _maxCarryingCapacity = maxCarryingCapacity;
        _currentCarryingCapacity = 0;
    }
    public void AddContainer(Container container)
    {
        /* Let's just call it "reserving weight" for future use,
         it would be hard for container to know how much carrying
        capacity the ship it's in has left*/
        if ((container.Weight + container.MaximumPayload)/1000.0 + _currentCarryingCapacity <= _maxCarryingCapacity && 
            _containers.Count < _maxContainerCount)
        {
            _containers.Add(container);
            _currentCarryingCapacity += (container.Weight + container.MaximumPayload)/1000.0;
        }
        else if (_containers.Count == _maxContainerCount)
        {
            throw new Exception("Counter count at maximum value");
        }
        else
        {
            throw new Exception("Counter carrying capacity could and/or would be exceeded");
        }
    }
    public void AddContainer(List<Container> newContainers)
    {
        foreach (Container c in newContainers)
        {
            AddContainer(c);
        }
    }
    
    public Container? RemoveContainer(string seriesNumber)
    {
        foreach (Container c in _containers)
        {
            if (!c.SeriesNumber.ToUpper().Equals(seriesNumber.ToUpper())) continue;
            _containers.Remove(c);
            _currentCarryingCapacity -= (c.Weight + c.MaximumPayload) / 1000.0;
            return c;
        }

        return null;
    }

    public Container? GetContainer(string seriesNumber)
    {
        foreach (Container c in _containers)
        {
            if (!c.SeriesNumber.ToUpper().Equals(seriesNumber.ToUpper())) continue;
            _containers.Remove(c);
            _currentCarryingCapacity -= (c.Weight + c.MaximumPayload) / 1000.0;
            return c;
        }
        
        return null;
    }

    public void ShowContainers()
    {
        int counter = 0;
        foreach (Container c in _containers)
        {
            Console.WriteLine($"{++counter}. {c.ToString()}");
        }
    }

    public new string ToString()
    {
        return $"Containers: {_containers.Count}, Max Containers: {_maxContainerCount}, " +
               $"Max Carry Capacity: {_maxCarryingCapacity}t, Max Spped: {_maxSpeed} knot{(_maxSpeed > 1.0f ? "s" : "")}";
    }
    
}