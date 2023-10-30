namespace SlowClap.Tests;

public class VariantUnitTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldSetProperties()
    {
        // Arrange
        string name = "TestVariant";
        int weight = 50;

        // Act
        var variant = new Variant(name, weight);

        // Assert
        Assert.Equal(name, variant.Name);
        Assert.Equal(weight, variant.Weight);
    }
    
    [Theory]
    [InlineData(null, 50)]
    [InlineData("", 50)]
    [InlineData("   ", 50)]
    public void Constructor_WithNullOrWhiteSpaceName_ShouldThrowArgumentException(string name, int weight)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Variant(name, weight));
    }
    
    [Theory]
    [InlineData("ValidName", -1)]
    [InlineData("ValidName", 101)]
    public void Constructor_WithInvalidWeight_ShouldThrowArgumentOutOfRangeException(string name, int weight)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Variant(name, weight));
    }
    
    [Fact]
    public void Name_Setter_WithValidName_ShouldSetProperty()
    {
        // Arrange
        var variant = new Variant("InitialName", 50);

        // Act
        variant.Name = "NewName";

        // Assert
        Assert.Equal("NewName", variant.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_Setter_WithNullOrWhiteSpaceName_ShouldThrowArgumentException(string newName)
    {
        // Arrange
        var variant = new Variant("InitialName", 50);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => variant.Name = newName);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Weight_Setter_WithInvalidWeight_ShouldThrowArgumentOutOfRangeException(int newWeight)
    {
        // Arrange
        var variant = new Variant("TestVariant", 50);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => variant.Weight = newWeight);
    }
    
    [Fact]
    public void Weight_Setter_WithValidWeight_ShouldSetProperty()
    {
        // Arrange
        var variant = new Variant("TestVariant", 50);

        // Act
        variant.Weight = 75;

        // Assert
        Assert.Equal(75, variant.Weight);
    }
}