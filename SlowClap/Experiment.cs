#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace SlowClap;

public class Experiment
{
    private string _name;
    
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
    public ICollection<Variant> Variants
    {
        get => _variants;
        set => _variants = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public Experiment(string name)
    {
        Name = name;
    }
    
    public Experiment(string name, ICollection<Variant> variants)
    {
        Name = name;
        Variants = variants;
    }
    
    public Experiment(string name, params Variant[] variants)
    {
        Name = name;
        Variants = variants;
    }
    
    public Experiment AddVariant(Variant variant)
    {
        Variants.Add(variant);
        return this;
    }
    
    public Experiment AddVariant(string name, int weight)
    {
        Variants.Add(new Variant(name, weight));
        return this;
    }

    public Variant? GetVariant(string name)
    {
        return Variants.FirstOrDefault(v => v.Name == name);
    }
    
    public Variant GetVariant(int index)
    {
        return Variants.ElementAt(index);
    }

    public bool RemoveVariant(string name)
    {
        var variant = GetVariant(name);
        return variant != null && Variants.Remove(variant);
    }
    
    public bool CheckIfExperimentIsValid()
    {
        return Variants.Count>=1 && Variants.Sum(v => v.Weight) == 100;
    }
    
}