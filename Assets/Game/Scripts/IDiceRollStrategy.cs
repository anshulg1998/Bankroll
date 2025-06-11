using UnityEngine;

/// <summary>
/// Interface for dice roll strategies. Allows for different dice rolling mechanics.
/// </summary>
public interface IDiceRollStrategy
{
    /// <summary>
    /// Rolls the dice and returns the result.
    /// </summary>
    /// <returns>The result of the dice roll.</returns>
    int Roll();
}

/// <summary>
/// Standard single dice roll strategy (1-6).
/// </summary>
public class StandardDiceRoll : IDiceRollStrategy
{
    /// <inheritdoc/>
    public int Roll() => UnityEngine.Random.Range(1, 7);
}

/// <summary>
/// Double dice roll strategy (sum of two dice, 2-12).
/// </summary>
public class DoubleDiceRoll : IDiceRollStrategy
{
    /// <inheritdoc/>
    public int Roll()
    {
        // Simulate rolling two dice and returning the sum
        return UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
    }
}
