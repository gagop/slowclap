namespace SlowClap.Tests;

public class AbTesterUnitTests
{
    [Fact]
    public void ChooseRandomVariant_WithValidExperiment_ShouldReturnVariant()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 50);
        experiment.AddVariant("Variant2", 50);

        // Act
        var result = tester.ChooseRandomVariant(experiment);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, experiment.Variants);
    }
    
    [Fact]
    public void ChooseRandomVariant_WithValidExperimentAndMultipleVariants_ShouldReturnVariant()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 25);
        experiment.AddVariant("Variant2", 25);
        experiment.AddVariant("Variant3", 50);

        // Act
        var result = tester.ChooseRandomVariant(experiment);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, experiment.Variants);
    }
    
    [Fact]
    public void ChooseConsistentVariant_WithValidArguments_ShouldReturnVariant()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 50);
        experiment.AddVariant("Variant2", 50);
        string userId = "user123";

        // Act
        var result = tester.ChooseConsistentVariant(experiment, userId);
        var result2 = tester.ChooseConsistentVariant(experiment, userId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, experiment.Variants);
        Assert.Equal(result, result2);
    }
    
    [Fact]
    public void ChooseRandomVariant_WithInvalidExperiment_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tester.ChooseRandomVariant(experiment));
    }
    
    [Fact]
    public void ChooseConsistentVariant_WithInvalidExperiment_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tester.ChooseConsistentVariant(experiment, "user123"));
    }
    
    [Fact]
    public void ChooseConsistentVariant_WithNullOrWhiteSpaceUserId_ShouldThrowArgumentException()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => tester.ChooseConsistentVariant(experiment, null));
        Assert.Throws<ArgumentException>(() => tester.ChooseConsistentVariant(experiment, ""));
        Assert.Throws<ArgumentException>(() => tester.ChooseConsistentVariant(experiment, "   "));
    }
 
    [Fact]
    public void ChooseRandomVariant_WithExperimentSumNotEqualTo100_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 60);
        experiment.AddVariant("Variant2", 45);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tester.ChooseRandomVariant(experiment));
    }
 
    [Fact]
    public void ChooseConsistentVariant_WithExperimentSumNotEqualTo100_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var tester = new AbTester();
        var experiment = new Experiment("TestExperiment");
        experiment.AddVariant("Variant1", 60);
        experiment.AddVariant("Variant2", 45);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tester.ChooseConsistentVariant(experiment, "user123"));
    }
    
}