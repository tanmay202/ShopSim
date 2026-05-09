using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class Plateau : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform foodpositionParent;
    [SerializeField] private int maxcapacity;

    [Header("Settings")]
    public bool isFull;
    public bool IsFull => isFull;

    /// <summary>
    /// True when no FoodPosition on this plateau holds any food.
    /// </summary>
    public bool IsEmpty => GetLastFullPosition() == null;

    private float positionsYOffset;

    public void Push(SpawnableFood foodinstance)
    {
        FoodPosition foodPosition = GetEmptyFoodPosition();
        foodPosition.Push(foodinstance);

        ReArrangeFoodPosition(foodinstance);

        if (GetEmptyFoodPosition() == null)
        {
            if (foodpositionParent.childCount < maxcapacity)
                CreateNewFoodPosition();
            else
                isFull = true;
        }
    }

    private void CreateNewFoodPosition()
    {
        FoodPosition foodPositionInstance = new GameObject("Food position" + foodpositionParent.childCount)
            .AddComponent<FoodPosition>();
        foodPositionInstance.transform.SetParent(foodpositionParent);

        int bottomchildIndex = foodPositionInstance.transform.GetSiblingIndex() - 1;
        foodPositionInstance.transform.localPosition =
            foodpositionParent.GetChild(bottomchildIndex).localPosition + Vector3.up * positionsYOffset;

        foodPositionInstance.transform.localRotation = quaternion.identity;
        isFull = false;
    }

    private void ReArrangeFoodPosition(SpawnableFood foodinstance)
    {
        positionsYOffset = foodinstance.CleanYOffsetonPlateau;

        for (int i = 0; i < foodpositionParent.childCount; i++)
        {
            foodpositionParent.GetChild(i).localPosition = Vector3.up * i * positionsYOffset;
        }
    }

    private FoodPosition GetEmptyFoodPosition()
    {
        for (int i = 0; i < foodpositionParent.childCount; i++)
        {
            if (!foodpositionParent.GetChild(i).TryGetComponent(out FoodPosition foodPosition))
                continue;

            if (foodPosition.IsEmpty)
                return foodPosition;
        }
        return null;
    }

    void Start()
    {
        isFull = false;
    }

    void Update()
    {
    }

    public SpawnableFood Pop()
    {
        FoodPosition foodPosition = GetLastFullPosition();

        if (foodPosition == null)
            return null;

        isFull = false;

        return foodPosition.Pop();
    }

    private FoodPosition GetLastFullPosition()
    {
        for (int i = foodpositionParent.childCount - 1; i >= 0; i--)
        {
            if (!foodpositionParent.GetChild(i).TryGetComponent(out FoodPosition foodPosition))
                continue;

            if (!foodPosition.IsEmpty)
                return foodPosition;
        }
        return null;
    }
}