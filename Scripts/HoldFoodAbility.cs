using UnityEngine;

public class HoldFoodAbility : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Plateau plateau;

    private const float canGrabFoodDelay = 0.1f;
    private float grabFoodTimer;

    /// <summary>
    /// True when the player's plateau has no food to give.
    /// Used by DropZone to check before attempting a drop.
    /// </summary>
    public bool IsEmpty => plateau == null || !plateau.gameObject.activeSelf || plateau.IsEmpty;

    private void Update()
    {
        grabFoodTimer += Time.deltaTime;
    }

    public void HandleFoodSpawnerStation(FoodSpawnerStation station)
    {
        // Delay check
        if (grabFoodTimer < canGrabFoodDelay)
            return;

        if (plateau.IsFull)
            return;

        SpawnableFood foodPrefab = station.Pop();

        if (foodPrefab == null)
            return;

        plateau.gameObject.SetActive(true);
        plateau.Push(foodPrefab);

        // Reset timer after successful grab
        grabFoodTimer = 0f;
    }

    /// <summary>
    /// Pops one food item off the player's plateau.
    /// Hides the plateau GameObject if it becomes empty after popping.
    /// </summary>
    public SpawnableFood Pop()
    {
        SpawnableFood food = plateau.Pop();

        if (food == null)
            return null;

        // Hide plateau if nothing left on it
        if (plateau.IsEmpty)
            plateau.gameObject.SetActive(false);

        return food;
    }
}