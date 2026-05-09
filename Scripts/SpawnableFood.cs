using UnityEngine;

public abstract class SpawnableFood : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float cleanYOffsetonPlateau;
  public float CleanYOffsetonPlateau=>cleanYOffsetonPlateau;


}
