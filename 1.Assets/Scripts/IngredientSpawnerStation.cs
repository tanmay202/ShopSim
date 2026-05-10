using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Ingredient depot station.
///
/// Place this on a GameObject that has:
///   • A BoxCollider (Is Trigger = true) — PlayerDetector fires on this.
///   • A child Plateau GameObject for stacking ingredients.
///   • (Optional) A Transform spawnPoint child that marks where
///     instantiated ingredients appear before being pushed onto the plateau.
///
/// In Play Mode, press [Generate Coffee Ingredients] in the inspector
/// to create <spawnAmount> Coffee prefabs on the plateau.
/// The player then walks into the trigger and grabs them one at a time,
/// identical to picking up food from a FoodSpawnerStation.
/// </summary>
public class IngredientSpawnerStation : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Coffee coffeePrefab;
    [SerializeField] private Plateau plateau;

    /// <summary>
    /// Optional world-space anchor where instantiated Coffee GameObjects
    /// are created before being parented to the plateau. If left empty,
    /// this transform is used instead.
    /// </summary>
    [SerializeField] private Transform spawnPoint;

    [Header("Settings")]
    [SerializeField] private int spawnAmount = 5;

    // ─────────────────────────────────────────────────────────────────
    //  Unity callbacks
    // ─────────────────────────────────────────────────────────────────

    private void Awake()
    {
        // No ingredients at startup — hidden until the button is pressed.
        if (plateau != null)
            plateau.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────────────────────────────
    //  Odin inspector button
    // ─────────────────────────────────────────────────────────────────

    [Button("Generate Coffee Ingredients"), GUIColor(0.4f, 0.9f, 0.4f)]
    public void GenerateIngredients()
    {
        if (coffeePrefab == null || plateau == null)
        {
            Debug.LogWarning($"[IngredientSpawnerStation] Missing references on {name}. " +
                             "Assign coffeePrefab and plateau in the inspector.");
            return;
        }

        plateau.gameObject.SetActive(true);

        Transform parent = spawnPoint != null ? spawnPoint : transform;

        for (int i = 0; i < spawnAmount; i++)
        {
            if (plateau.IsFull)
                break;

            Coffee coffeeInstance = Instantiate(coffeePrefab, parent);
            plateau.Push(coffeeInstance);
        }
    }

    // ─────────────────────────────────────────────────────────────────
    //  Public API — called by HoldFoodAbility.HandleIngredientSpawner
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Pops one Coffee ingredient off the plateau for the player.
    /// Hides the plateau GameObject when it becomes empty.
    /// </summary>
    public SpawnableFood Pop()
    {
        SpawnableFood ingredient = plateau.Pop();

        if (plateau.IsEmpty)
            plateau.gameObject.SetActive(false);

        return ingredient;
    }

    /// <summary>True when no ingredients are left on the plateau.</summary>
    public bool IsEmpty => plateau == null || plateau.IsEmpty;
}
