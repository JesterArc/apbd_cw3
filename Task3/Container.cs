namespace Task3;
public enum LiquidPayloadType
{
    Dangerous,
    Safe,
    None
}

public class OverfillException() : Exception("Maximum payload size exceeded.");


public class Container
{
    public double CurrentPayload { get; protected set; }
    private static int _id;
    public double Height { get; protected set; }
    public double Weight { get; protected set; }
    public double Depth { get; protected set; }
    public string ContainerType { get; protected set; }
    public string SeriesNumber { get; protected set; }
    public double MaximumPayload { get; protected set; }
    public Container(double height, double weight, double depth, string containerType, double maximumPayload)
    {
        Height = height;
        Weight = weight;
        Depth = depth;
        MaximumPayload = maximumPayload;
        ContainerType = containerType;
        SeriesNumber = $"KON-{containerType}-{++_id}";
    }
    public virtual void Unload()
    {
        CurrentPayload = 0;
    }

    public virtual void Load(double payload)
    {
        if (payload > MaximumPayload - CurrentPayload)
        {
            throw new OverfillException();
        }
        CurrentPayload += payload;
    }

    public new virtual string ToString()
    {
        return $"SeriesNumber: {SeriesNumber}, Height: {Height}cm, Weight: {Weight}kg, Depth: {Depth}cm" +
               $", CurrentPayload: {CurrentPayload}kg, MaximumPayload: {MaximumPayload}kg";
    }
}

public class LiquidContainer(double height, double weight, double depth, double maximumPayload)
    : Container(height, weight, depth, "L", maximumPayload), IHazardNotifier
{
    private LiquidPayloadType _liquidPayloadType = LiquidPayloadType.None;

    public override void Unload()
    {
        CurrentPayload = 0;
        _liquidPayloadType = LiquidPayloadType.None;
    }

    public override void Load(double payload)
    {
        Load(payload, LiquidPayloadType.Safe);
    }
    public void Load(double payload, LiquidPayloadType liquidPayloadType)
    {
        if (_liquidPayloadType != liquidPayloadType && _liquidPayloadType != LiquidPayloadType.None)
        {
            Console.WriteLine("Incompatible liquid payload type." +
                                     $"Current LiquidPayloadType: {_liquidPayloadType}.");
            Notify();
        }
        else
        {
            _liquidPayloadType = liquidPayloadType;
            if (payload > MaximumPayload - CurrentPayload)
            {
                throw new OverfillException();
            }
            switch (_liquidPayloadType)
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
    public override string ToString()
    {
        return base.ToString() + $", LiquidPayloadType: {_liquidPayloadType}";
    }
}

public class GasContainer
    : Container, IHazardNotifier
{
    public double Atmosphere { get; protected set; }
    public GasContainer(double height, double weight, double depth, double maximumPayload, double atmosphere)
        : base(height, weight, depth, "G", maximumPayload)
    {
        Atmosphere = atmosphere;
    }
    public override void Unload()
    {
        CurrentPayload = 0.05 * CurrentPayload;
        
    }
    public void Notify()
    {
        Console.Error.WriteLine($"Hazard detected, Container: {SeriesNumber}");
    }

    public override string ToString()
    {
        return base.ToString() + $", Atmosphere: {Atmosphere}Pa";
    }
}

public class ChilledContainer : Container
{
    private readonly Dictionary<string, double> _productList;
    private readonly double _temperature;
    private readonly string _product;
    public ChilledContainer(double height, double weight, double depth, double maximumPayload, double temperature,
        string product, Dictionary<string, double> dict) :
        base(height, weight, depth, "C", maximumPayload)
    {
        _productList = dict;
        if (!_productList.ContainsKey(product))
        {
            throw new Exception("Product does not exist.");
        }
        _product = product;
        if (temperature < _productList.GetValueOrDefault(product))
        {
            throw new Exception("Temperature of container is too low for this product.");
        }
        _temperature = temperature;
    }

    public override string ToString()
    {
        return base.ToString() + $", Temperature: {_temperature}\u00b0C, Product: {_product}";
    }
}