using UnityEngine;

/// <summary>
/// Coffee machine station.
///
/// Flow:
///   1. Player carries a Coffee ingredient and enters the trigger zone.
///   2. Machine auto-accepts one Coffee, destroys it, and pushes
///      <cupsPerCoffee> CoffieCup instances onto outputPlateau.
///   3. Player re-enters (or stays) and the machine dispenses cups
///      one at a time onto the player's plateau — same rate as
///      grabbing from a FoodSpawnerStation.
///
/// Setup:
///   • Attach a BoxCollider (Is Trigger = true) to this GameObject.
///   • Assign coffieCupPrefab (CoffieCup prefab with a CleanYOffsetonPlateau value).
///   • Assign outputPlateau (a child Plateau that will hold generated cups).
///     The plateau GameObject will be shown/hidden automatically.
/// </summary>
public class CoffeeMachine : Machine
{
    [Header("Elements")]
    [SerializeField] private CoffieCup coffieCupPrefab;
    [SerializeField] private Plateau outputPlateau;

    [Header("Settings")]
    [SerializeField] private int cupsPerCoffee = 7;

    /// <summary>
    /// Minimum seconds between two consecutive player interactions
    /// (accepting an ingredient OR dispensing a cup).
    /// </summary>
    [SerializeField] private float interactionDelay = 0.2f;

    private float interactionTimer;

    // ─────────────────────────────────────────────────────────────────
    //  Unity callbacks
    // ─────────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Start with the output plateau hidden — no cups yet.
        if (outputPlateau != null)
            outputPlateau.gameObject.SetActive(false);
    }

    private void Update()
    {
        interactionTimer += Time.deltaTime;
    }

    // ─────────────────────────────────────────────────────────────────
    //  Machine override
    // ─────────────────────────────────────────────────────────────────

    public override void HandlePlayerInteraction(HoldFoodAbility holdFoodAbility)
    {
        if (interactionTimer < interactionDelay)
            return;

        // Priority 1 — accept a Coffee ingredient from the player.
        if (TryAcceptCoffeeIngredient(holdFoodAbility))
            return;

        // Priority 2 — dispense a finished cup to the player.
        TryDispenseCup(holdFoodAbility);
    }

    // ─────────────────────────────────────────────────────────────────
    //  Private helpers
    // ─────────────────────────────────────────────────────────────────

    /// <returns>True when a Coffee was successfully consumed.</returns>
    private bool TryAcceptCoffeeIngredient(HoldFoodAbility holdFoodAbility)
    {
        // Peek before popping so we don't accidentally steal other food types.
        SpawnableFood topItem = holdFoodAbility.Peek();

        if (topItem == null || topItem is not Coffee)
            return false;

        // Consume the ingredient.
        SpawnableFood coffee = holdFoodAbility.Pop();
        Destroy(coffee.gameObject);

        GenerateCups();

        interactionTimer = 0f;
        return true;
    }

    private void GenerateCups()
    {
        outputPlateau.gameObject.SetActive(true);

        for (int i = 0; i < cupsPerCoffee; i++)
        {
            if (outputPlateau.IsFull)
                break;

            CoffieCup cup = Instantiate(coffieCupPrefab, transform);
            outputPlateau.Push(cup);
        }
    }

    private void TryDispenseCup(HoldFoodAbility holdFoodAbility)
    {
        if (outputPlateau.IsEmpty)
            return;

        // HoldFoodAbility handles the rate-limit & plateau activation.
        holdFoodAbility.HandleMachineOutput(this);
    }

    // ─────────────────────────────────────────────────────────────────
    //  Public API — called by HoldFoodAbility.HandleMachineOutput
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Pops one cup off the output plateau.
    /// Hides the plateau GameObject when it becomes empty.
    /// </summary>
    public SpawnableFood Pop()
    {
        SpawnableFood cup = outputPlateau.Pop();

        if (outputPlateau.IsEmpty)
            outputPlateau.gameObject.SetActive(false);

        return cup;
    }

    /// <summary>True when there are no generated cups left to dispense.</summary>
    public bool IsEmpty => outputPlateau == null || outputPlateau.IsEmpty;
}
