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
        // Grab food from a spawner station
        if (other.TryGetComponent(out FoodSpawnerStation spawnerStation))
        {
            holdFoodAbility.HandleFoodSpawnerStation(spawnerStation);
            return;
        }

        // Drop food onto a cashier / drop zone plateau
        if (other.TryGetComponent(out DropZone dropZone))
        {
            dropZone.HandlePlayerDrop(holdFoodAbility);
        }
    }
}