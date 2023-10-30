namespace SlowClap;

/// <summary>
/// Represents a variant in an A/B testing experiment with a specific name and weight.
/// </summary>
public class Variant
{
    private string _name;

    /// <summary>
    /// Gets or sets the name of the variant. Cannot be null or whitespace.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when attempting to set a null or whitespace name.</exception>
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

    /// <summary>
    /// Gets or sets the weight of the variant. Must be between 0 and 100.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when attempting to set a weight outside the valid range [0, 100].</exception>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="Variant"/> class with the specified name and weight.
    /// </summary>
    /// <param name="name">The name of the variant.</param>
    /// <param name="weight">The weight of the variant (between 0 and 100).</param>
    public Variant(string name, int weight)
    {
        Name = name;
        Weight = weight;
    }
    
}