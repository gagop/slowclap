namespace SlowClap.Tests;

public class ExperimentUnitTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldSetProperties()
    {
        // Arrange
        string name = "TestExperiment";
        var variants = new List<Variant>
        {
            new Variant("Variant1", 50),
            new Variant("Variant2", 50)
        };

        // Act
        var experiment = new Experiment(name, variants);

        // Assert
        Assert.Equal(name, experiment.Name);
        Assert.Equal(variants, experiment.Variants);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceName_ShouldThrowArgumentException(string name)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Experiment(name));
    }
    
    [Fact]
    public void AddVariant_WithValidVariant_ShouldAddToVariants()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");
        var variant = new Variant("NewVariant", 25);

        // Act
        experiment.AddVariant(variant);

        // Assert
        Assert.Contains(variant, experiment.Variants);
    }

    [Fact]
    public void AddVariant_WithValidArguments_ShouldAddVariantToVariants()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");

        // Act
        experiment.AddVariant("NewVariant", 25);

        // Assert
        Assert.Equal(1, experiment.Variants.Count);
        Assert.Equal("NewVariant", experiment.Variants.First().Name);
    }
    
    [Fact]
    public void GetVariant_WithName_ShouldReturnVariant()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");
        var variant = new Variant("ExistingVariant", 50);
        experiment.AddVariant(variant);

        // Act
        var result = experiment.GetVariant("ExistingVariant");

        // Assert
        Assert.Equal(variant, result);
    }
 
    [Fact]
    public void GetVariant_WithIndex_ShouldReturnVariant()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");
        var variant = new Variant("ExistingVariant", 50);
        experiment.AddVariant(variant);

        // Act
        var result = experiment.GetVariant(0);

        // Assert
        Assert.Equal(variant, result);
    }
    
    [Fact]
    public void RemoveVariant_WithExistingVariant_ShouldRemoveFromVariants()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");
        var variant = new Variant("ExistingVariant", 50);
        experiment.AddVariant(variant);

        // Act
        var result = experiment.RemoveVariant("ExistingVariant");

        // Assert
        Assert.True(result);
        Assert.Empty(experiment.Variants);
    }

    [Fact]
    public void RemoveVariant_WithNonExistingVariant_ShouldReturnFalse()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");

        // Act
        var result = experiment.RemoveVariant("NonExistingVariant");

        // Assert
        Assert.False(result);
    }
 
    [Fact]
    public void CheckIfExperimentIsValid_WithValidExperiment_ShouldReturnTrue()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 50);
        experiment.AddVariant("Variant2", 50);

        // Act
        var result = experiment.CheckIfExperimentIsValid();

        // Assert
        Assert.True(result);
    }
     

    [Fact]
    public void ChooseRandomVariant_ValidExperimentWithTwoVariants_ReturnsVariant()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment", new Variant("Variant1", 50), new Variant("Variant2", 50));

        // Act
        var chosenVariant = experiment.ChooseRandomVariant();

        // Assert
        Assert.Contains(chosenVariant, experiment.Variants);
    }

    [Fact]
    public void ChooseRandomVariant_ValidExperimentWithThreeVariants_ReturnsVariant()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment", new Variant("Variant1", 25), new Variant("Variant2", 25), new Variant("Variant2", 50));

        // Act
        var chosenVariant = experiment.ChooseRandomVariant();

        // Assert
        Assert.Contains(chosenVariant, experiment.Variants);
    }

    [Fact]
    public void ChooseConsistentVariant_ValidExperimentWithTwoVariants_ReturnsVariant()
    {
        // Arrange
        var experiment = new Experiment("TestExperiment", new Variant("Variant1", 50), new Variant("Variant2", 50));

        // Act
        var chosenVariant = experiment.ChooseConsistentVariant("abc123");

        // Assert
        Assert.Contains(chosenVariant, experiment.Variants);
    }

    
    [Fact]
    public void GetRandomInt_ReturnsValueWithinRange()
    {
        for (int i = 0; i < 1000; i++)
        {
            int result = Experiment.GetRandomInt();
            Assert.True(result >= 0, $"Out of range value: {result}");
        }
    }

    [Fact]
    public void GetRandomInt_ThreadSafetyTest()
    {
        int tasksCount = 100;
        Task[] tasks = new Task[tasksCount];

        for (int i = 0; i < tasksCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 100; j++)
                {
                    Experiment.GetRandomInt();
                }
            });
        }

        Task.WhenAll(tasks).Wait();
        // No exceptions mean it's thread-safe under the given test conditions
    }

    [Fact]
    public void GetRandomInt_GeneratesDifferentValues()
    {
        int firstValue = Experiment.GetRandomInt();
        int differentValuesCount = 0;

        for (int i = 0; i < 100; i++)
        {
            if (Experiment.GetRandomInt() != firstValue) differentValuesCount++;
        }

        Assert.True(differentValuesCount > 0, "Generated values are too similar");
    }
}