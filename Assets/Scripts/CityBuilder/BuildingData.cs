using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "BuildingData/Building")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public Sprite Preview { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Model { get; private set; }
    [field: SerializeField] public BuildingType Type { get; private set; }   
}