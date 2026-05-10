using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private HoldFoodAbility holdFoodAbility;

    private void Awake()
    {
        holdFoodAbility = GetComponent<HoldFoodAbility>();
    }

    private void OnTriggerStay(Collider other)
    {
        // ── Food source ───────────────────────────────────────────────
        // Grab food from a regular spawner station.
        if (other.TryGetComponent(out FoodSpawnerStation spawnerStation))
        {
            holdFoodAbility.HandleFoodSpawnerStation(spawnerStation);
            return;
        }

        // ── Ingredient source ─────────────────────────────────────────
        // Grab coffee (or other ingredients) from an ingredient depot.
        if (other.TryGetComponent(out IngredientSpawnerStation ingredientSpawner))
        {
            holdFoodAbility.HandleIngredientSpawner(ingredientSpawner);
            return;
        }

        // ── Machine interaction ───────────────────────────────────────
        // Handles both depositing ingredients AND collecting output
        // (e.g. dropping Coffee into a CoffeeMachine, then grabbing cups).
        // Checked before DropZone so machines get priority.
        if (other.TryGetComponent(out Machine machine))
        {
            machine.HandlePlayerInteraction(holdFoodAbility);
            return;
        }

        // ── Drop zone ─────────────────────────────────────────────────
        // Drop food onto a cashier / counter plateau.
        if (other.TryGetComponent(out DropZone dropZone))
        {
            dropZone.HandlePlayerDrop(holdFoodAbility);
        }
    }
}
