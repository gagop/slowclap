namespace SlowClap;

public class Variant
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Variant name cannot be null or whitespace", nameof(value));
            }

            _name = value;
        }
    }

    private int _weight;
    
    public int Weight
    {
        get => _weight;
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Weight must be between 0 and 100");
            }

            _weight = value;
        }
    }

    public Variant(string name, int weight)
    {
        Name = name;
        Weight = weight;
    }
    
}