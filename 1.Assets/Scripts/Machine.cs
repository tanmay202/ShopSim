using UnityEngine;

/// <summary>
/// Abstract base for all station machines (CoffeeMachine, etc.).
/// PlayerDetector calls HandlePlayerInteraction when the player
/// is inside the machine's trigger collider.
/// </summary>
public abstract class Machine : MonoBehaviour
{
    /// <summary>
    /// Called every fixed frame by PlayerDetector while the player
    /// overlaps this machine's trigger zone.
    /// </summary>
    public abstract void HandlePlayerInteraction(HoldFoodAbility holdFoodAbility);
}
