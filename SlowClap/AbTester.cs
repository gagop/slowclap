namespace SlowClap;

public class AbTester
{
    private readonly Random _random = new();

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