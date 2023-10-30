#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace SlowClap;

/// <summary>
/// A controlled experiment with a name and multiple variants is represented by this class. Names must not be null or contain whitespace. Methods for variant manipulation, validity checks, and convenient initialization are provided.
/// </summary>
public class Experiment
{
    private string _name;

    /// <summary>
    /// Represents the name of the experiment. Ensures non-null and non-whitespace values.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Experiment name cannot be null or whitespace.", nameof(value));
            }
            _name = value;
        }
    }

    private ICollection<Variant> _variants = new List<Variant>();

    /// <summary>
    /// Gets or sets the collection of variants associated with the experiment. Ensures non-null assignment. Use this property to manage experiment variants, allowing addition, removal, and retrieval. The collection must collectively have a total weight of 100 for experiment validity.
    /// </summary>
    public ICollection<Variant> Variants
    {
        get => _variants;
        set => _variants = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public Experiment(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes an experiment with a specified name and a collection of variants.
    /// </summary>
    /// <param name="name">The name of the experiment.</param>
    /// <param name="variants">The collection of variants associated with the experiment.</param>
    public Experiment(string name, ICollection<Variant> variants)
    {
        Name = name;
        Variants = variants;
    }

    /// <summary>
    /// Initializes an experiment with a specified name and an array of variants.
    /// </summary>
    /// <param name="name">The name of the experiment.</param>
    /// <param name="variants">The array of variants associated with the experiment.</param>
    public Experiment(string name, params Variant[] variants)
    {
        Name = name;
        Variants = variants;
    }

    /// <summary>
    /// Adds a variant to the experiment's collection and returns the updated experiment.
    /// </summary>
    /// <param name="variant">The variant to add.</param>
    /// <returns>The experiment with the added variant.</returns>
    public Experiment AddVariant(Variant variant)
    {
        Variants.Add(variant);
        return this;
    }

    /// <summary>
    /// Adds a new variant with the given name and weight to the experiment's collection and returns the updated experiment.
    /// </summary>
    /// <param name="name">The name of the new variant.</param>
    /// <param name="weight">The weight of the new variant. It should be a value between 0 and 100 (inclusive).</param>
    /// <returns>The experiment with the added variant.</returns>
    public Experiment AddVariant(string name, int weight)
    {
        Variants.Add(new Variant(name, weight));
        return this;
    }

    /// <summary>
    /// Retrieves a variant by name from the experiment's collection. Returns null if the variant is not found.
    /// </summary>
    /// <param name="name">The name of the variant to retrieve.</param>
    /// <returns>The variant with the specified name, or null if not found.</returns>
    public Variant? GetVariant(string name)
    {
        return Variants.FirstOrDefault(v => v.Name == name);
    }

    /// <summary>
    /// Retrieves a variant from the experiment's collection by index.
    /// </summary>
    /// <param name="index">The index of the variant to retrieve.</param>
    /// <returns>The variant at the specified index.</returns>
    public Variant GetVariant(int index)
    {
        return Variants.ElementAt(index);
    }

    /// <summary>
    /// Removes a variant by name from the experiment's collection. Returns true if successful, false otherwise.
    /// </summary>
    /// <param name="name">The name of the variant to remove.</param>
    /// <returns>True if the variant was removed, false if the variant was not found.</returns>
    public bool RemoveVariant(string name)
    {
        var variant = GetVariant(name);
        return variant != null && Variants.Remove(variant);
    }

    /// <summary>
    /// Checks if the experiment is valid, i.e., it has at least one variant and the total weight of variants is 100.
    /// </summary>
    /// <returns>True if the experiment is valid, false otherwise.</returns>
    public bool CheckIfExperimentIsValid()
    {
        return Variants.Count>=1 && Variants.Sum(v => v.Weight) == 100;
    }
    
}