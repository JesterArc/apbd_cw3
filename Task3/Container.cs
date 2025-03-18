namespace Task3;

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
    protected Guid ContainerId;
    protected string SeriesNumber;
    protected readonly double MaximumPayload;

    public Container(double height, double weight, double depth, char containerType, double maximumPayload)
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