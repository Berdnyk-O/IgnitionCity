using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    public float Rotation => _wrapper.transform.eulerAngles.y;

    [SerializeField] private Transform _wrapper;

    private BuildingShapeUnit[] _shapeUnits;

    void Awake()
    {
        _shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float rotation)
    {
        _wrapper.Rotate(new Vector3(0, rotation, 0));
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return _shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}
