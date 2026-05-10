using UnityEngine;

/// <summary>
/// Attach this to the cashier station's plateau trigger collider GameObject.
/// The collider should be a Trigger.
/// </summary>
public class DropZone : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Plateau targetPlateau;

    [Header("Settings")]
    [SerializeField] private float dropDelay = 0.1f;
    private float dropTimer;

    private void Update()
    {
        dropTimer += Time.deltaTime;
    }

    /// <summary>
    /// Called by PlayerDetector when the player enters/stays in this drop zone.
    /// Transfers one food item from the player's plateau to this station's plateau.
    /// </summary>
    /// <param name="holdFoodAbility">The player's HoldFoodAbility component.</param>
    public void HandlePlayerDrop(HoldFoodAbility holdFoodAbility)
    {
        if (dropTimer < dropDelay)
            return;

        if (targetPlateau.IsFull)
            return;

        if (holdFoodAbility.IsEmpty)
            return;

        SpawnableFood food = holdFoodAbility.Pop();

        if (food == null)
            return;

        targetPlateau.gameObject.SetActive(true);
        targetPlateau.Push(food);

        dropTimer = 0f;
    }
}
