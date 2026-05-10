using System;
using UnityEngine;

public class FoodPosition : MonoBehaviour
{
    public SpawnableFood food;
    private bool isEmpty;
    public bool IsEmpty=>isEmpty;
     
    public void Push(SpawnableFood foodinstance)
    {
        food=foodinstance;
        foodinstance.transform.SetParent(transform);
        foodinstance.transform.localPosition=Vector3.zero;
        isEmpty=false;
    }

    public SpawnableFood Pop()
    {
        isEmpty=true;

        SpawnableFood foodToreturn =food;
        food=null;

        return foodToreturn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isEmpty=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
