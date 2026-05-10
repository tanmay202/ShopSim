using UnityEngine;

public class HoldFoodAbility : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Plateau plateau;

    private const float canGrabFoodDelay = 0.1f;
    private float grabFoodTimer;

    /// <summary>
    /// True when the player's plateau has no food/ingredient to give.
    /// Used by DropZone to check before attempting a drop.
    /// </summary>
    public bool IsEmpty => plateau == null || !plateau.gameObject.activeSelf || plateau.IsEmpty;

    private void Update()
    {
        grabFoodTimer += Time.deltaTime;
    }

    // ─────────────────────────────────────────────────────────────────
    //  Grab from spawner stations
    // ─────────────────────────────────────────────────────────────────

    public void HandleFoodSpawnerStation(FoodSpawnerStation station)
    {
        if (grabFoodTimer < canGrabFoodDelay)
            return;

        if (plateau.IsFull)
            return;

        SpawnableFood foodPrefab = station.Pop();

        if (foodPrefab == null)
            return;

        plateau.gameObject.SetActive(true);
        plateau.Push(foodPrefab);

        grabFoodTimer = 0f;
    }

    /// <summary>
    /// Grabs one Coffee ingredient from an IngredientSpawnerStation
    /// and pushes it onto the player's plateau.
    /// </summary>
    public void HandleIngredientSpawner(IngredientSpawnerStation station)
    {
        if (grabFoodTimer < canGrabFoodDelay)
            return;

        if (plateau.IsFull)
            return;

        SpawnableFood ingredient = station.Pop();

        if (ingredient == null)
            return;

        plateau.gameObject.SetActive(true);
        plateau.Push(ingredient);

        grabFoodTimer = 0f;
    }

    /// <summary>
    /// Grabs one item (e.g. a CoffieCup) from a CoffeeMachine's output
    /// and pushes it onto the player's plateau.
    /// </summary>
    public void HandleMachineOutput(CoffeeMachine machine)
    {
        if (grabFoodTimer < canGrabFoodDelay)
            return;

        if (plateau.IsFull)
            return;

        SpawnableFood item = machine.Pop();

        if (item == null)
            return;

        plateau.gameObject.SetActive(true);
        plateau.Push(item);

        grabFoodTimer = 0f;
    }

    // ─────────────────────────────────────────────────────────────────
    //  Read / remove from player plateau
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the topmost item on the player's plateau WITHOUT removing it.
    /// Used by CoffeeMachine to verify the item type before consuming it.
    /// Returns null when the plateau is empty or inactive.
    /// </summary>
    public SpawnableFood Peek()
    {
        if (plateau == null || !plateau.gameObject.activeSelf)
            return null;

        return plateau.Peek();
    }

    /// <summary>
    /// Pops one food/ingredient item off the player's plateau.
    /// Hides the plateau GameObject if it becomes empty after popping.
    /// </summary>
    public SpawnableFood Pop()
    {
        SpawnableFood food = plateau.Pop();

        if (food == null)
            return null;

        if (plateau.IsEmpty)
            plateau.gameObject.SetActive(false);

        return food;
    }
}
