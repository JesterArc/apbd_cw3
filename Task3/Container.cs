namespace Task3;
public enum LiquidPayloadType
{
    Dangerous,
    Safe,
    None
}
public class OverfillException : Exception
{
    public OverfillException() : base("Maximum payload size exceeded.")
    {
        
    }
}

public class Container
{
    protected double CurrentPayload;
    protected double Height;
    protected double Weight;
    protected double Depth;
    protected char ContainerType;
    private Guid ContainerId;
    public readonly string SeriesNumber;
    protected readonly double MaximumPayload;
    protected Container(double height, double weight, double depth, char containerType, double maximumPayload)
    {
        Height = height;
        Weight = weight;
        Depth = depth;
        MaximumPayload = maximumPayload;
        ContainerId = Guid.NewGuid();
        SeriesNumber = $"KON-${containerType}-${ContainerId}";
    }
    public void Unload()
    {
        CurrentPayload = 0;
    }

    public void Load(double payload)
    {
        if (payload > MaximumPayload - CurrentPayload)
        {
            throw new OverfillException();
        }
        CurrentPayload += payload;
    }
}

public class LiquidContainer(double height, double weight, double depth, double maximumPayload)
    : Container(height, weight, depth, 'L', maximumPayload), IHazardNotifier
{
    private LiquidPayloadType _currentLiquidPayloadType = LiquidPayloadType.None;

    public new void Unload()
    {
        CurrentPayload = 0;
        _currentLiquidPayloadType = LiquidPayloadType.None;
    }

    public new void Load(double payload, LiquidPayloadType liquidPayloadType)
    {
        if (_currentLiquidPayloadType != liquidPayloadType && _currentLiquidPayloadType != LiquidPayloadType.None)
        {
            Console.WriteLine("Incompatible liquid payload type." +
                                     $"Current LiquidPayloadType: {_currentLiquidPayloadType}.");
            Notify();
        }
        else
        {
            _currentLiquidPayloadType = liquidPayloadType;
            if (payload > MaximumPayload - CurrentPayload)
            {
                throw new OverfillException();
            }
            switch (_currentLiquidPayloadType)
            {
                case LiquidPayloadType.Dangerous:
                    if (payload > 0.5 * MaximumPayload - CurrentPayload)
                    {
                        Notify();
                    }
                    CurrentPayload += payload;
                    break;
                case LiquidPayloadType.Safe:
                    if (payload > 0.9 * MaximumPayload - CurrentPayload)
                    {
                        Notify();
                    }
                    CurrentPayload += payload;
                    break;
            }
        }
    }

    public void Notify()
    {
        Console.Error.WriteLine($"Hazard detected, Container: {SeriesNumber}");
    }
    
}

public class GasContainer(double height, double weight, double depth, double maximumPayload)
    : Container(height, weight, depth, 'G', maximumPayload), IHazardNotifier
{
    public new void Unload()
    {
        CurrentPayload = 0.05 * CurrentPayload;
        
    }
    public void Notify()
    {
        Console.Error.WriteLine($"Hazard detected, Container: {SeriesNumber}");
    }
}
