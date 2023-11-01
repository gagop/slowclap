# slowclap

The SlowClap A/B library is a small library designed to make implementing A/B tests in C# applications easier. It can also be used to gradually introduce new features into the application in order to detect any problems as soon as possible. Library is thread-safe.

# Installation

To get started with SlowClap, install the NuGet package using the following command:

```
dotnet add package Gago.SlowClap
```

# Example usage

```
// Create an experiment with variants
var experiment = new Experiment("ButtonColorTest")
    .AddVariant("RedButton", 50)
    .AddVariant("BlueButton", 50);

// All the variants should sum up to 100%.
// Based on this setting, we can control the probability of each variant occurring in the calculations.

// Perform random assignment
var randomVariant = experiment.ChooseRandomVariant();

// Or perform consistent assignment based on user ID (or other information stable for the user)
var userId = "user123";
var consistentVariant = experiment.ChooseConsistentVariant(userId);
```

# Future ideas

- Lifecycle management - Implement features for dynamically starting, pausing, and stopping experiments, allowing for more flexible experiment management.
- Targeting and segmentation - Within experiments, investigate features for segmenting user groups and targeting specific demographics or user attributes.
- Statistical Analysis (Advanced) - Improve statistical analysis capabilities to provide deeper insights into experiment results.
