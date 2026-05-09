using UnityEngine;

public class FoodSpawnerStation : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private SpawnableFood spawnableFoodPrefab;
    [SerializeField] private Plateau plateau;

    [Header("Settings")]
    [SerializeField] private float spawndelay;
    private float spawnTimer;

    private void Update()
    {
        HandleSpawnTimer();
    }

    private void HandleSpawnTimer()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawndelay)
        {
            TrySpawnFood();
            spawnTimer = 0f;
        }
    }

    private void TrySpawnFood()
    {
        if (plateau.IsFull)
            return;

        SpawnFood();
    }

    private void SpawnFood()
    {
        SpawnableFood foodInstance = Instantiate(spawnableFoodPrefab, transform);
        plateau.Push(foodInstance);
    }

    public SpawnableFood Pop()
    {
        return plateau.Pop();
    }
}