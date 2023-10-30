namespace SlowClap;

/// <summary>
/// A/B Tester for conducting experiments and choosing variants.
/// </summary>
public class AbTester
{
    private readonly Random _random = new();

    /// <summary>
    /// Chooses a random variant from the specified experiment based on variant weights.
    /// </summary>
    /// <param name="experiment">The experiment from which to choose a variant.</param>
    /// <returns>The randomly chosen variant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the experiment configuration is invalid.</exception>
    public Variant ChooseRandomVariant(Experiment experiment)
    {
        if (!experiment.CheckIfExperimentIsValid())
        {
            throw new InvalidOperationException("Invalid experiment configuration. Make sure that you have at least 1 variant and all the variants can be summed up to 100."); 
        }
        
        int roll = (int)(_random.NextDouble()*101);
        int sum = 0;

        foreach (var variant in experiment.Variants)
        {
            sum += variant.Weight;
            if (roll <= sum)
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
    public Variant ChooseConsistentVariant(Experiment experiment, string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or whitespace", nameof(userId));
        }
        
        if (!experiment.CheckIfExperimentIsValid())
        {
            throw new InvalidOperationException("Invalid experiment configuration. Make sure that you have at least 1 variant and all the variants can be summed up to 100."); 
        }
        
        int hash = Math.Abs(userId.GetHashCode());
        int hashNormalized = hash % 101; // This will give a value in the range [0, 100]
        
        int sum = 0;
        foreach (var variant in experiment.Variants)
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