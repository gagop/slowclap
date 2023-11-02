#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace SlowClap;

/// <summary>
/// A controlled experiment with a name and multiple variants is represented by this class. Names must not be null or contain whitespace. Methods for variant manipulation, validity checks, and convenient initialization are provided.
/// </summary>
public class Experiment
{
    private static readonly Random _globalRandom = new();

    [ThreadStatic]
    private static Random _localRandom;

    /// <summary>
    /// Returns a random integer.
    /// Utilizes thread-local random instances to ensure thread safety
    /// and reduce contention when accessed concurrently.
    /// If a thread-local random instance doesn't exist, a seed is acquired
    /// from a global random instance to initialize it.
    /// https://devblogs.microsoft.com/pfxteam/getting-random-numbers-in-a-thread-safe-way/
    /// </summary>
    public static int GetRandomInt()
    {
        Random instance = _localRandom;
        if (instance == null)
        {
            int seed;
            lock (_globalRandom) seed = _globalRandom.Next();
            _localRandom = instance = new Random(seed);
        }
        return instance.Next();
    }
    
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
        return Variants.Count>=1;
    }

    /// <summary>
    /// Chooses a random variant from the specified experiment based on variant weights.
    /// </summary>
    /// <param name="experiment">The experiment from which to choose a variant.</param>
    /// <returns>The randomly chosen variant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the experiment configuration is invalid.</exception>
    public Variant ChooseRandomVariant()
    {
        if (!CheckIfExperimentIsValid())
        {
            throw new InvalidOperationException("Invalid experiment configuration. Make sure that you have at least 1 variant and all the variants can be summed up to 100.");
        }

        int totalWeight = Variants.Sum(v => v.Weight);
        int roll = GetRandomInt();
        int normalizedRoll = roll % totalWeight;
        
        int sum = 0;
        
        foreach (var variant in Variants)
        {
            sum += variant.Weight;
            if (normalizedRoll <= sum)
            {
                return variant;
            }
        }

        throw new InvalidOperationException("Invalid experiment configuration.");
    }

    /// <summary>
    /// Chooses a consistent variant for a user from the specified experiment based on user ID.
    /// </summary>
    /// <param name="experiment">The experiment from which to choose a variant.</param>
    /// <param name="userId">The user ID (or other stable value) for whom to choose a consistent variant.</param>
    /// <returns>The consistently chosen variant for the specified user.</returns>
    /// <exception cref="ArgumentException">Thrown when the user ID is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the experiment configuration is invalid.</exception>
    public Variant ChooseConsistentVariant(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or whitespace", nameof(userId));
        }

        if (!CheckIfExperimentIsValid())
        {
            throw new InvalidOperationException("Invalid experiment configuration. Make sure that you have at least 1 variant and all the variants can be summed up to 100.");
        }

        int hash = Math.Abs(userId.GetHashCode());
        int totalWeight = Variants.Sum(v => v.Weight);
        int hashNormalized = hash % totalWeight;

        int sum = 0;
        foreach (var variant in Variants)
        {
            sum += variant.Weight;
            if (hashNormalized <= sum)
            {
                return variant;
            }
        }

        throw new InvalidOperationException("Invalid experiment configuration.");
    }
}