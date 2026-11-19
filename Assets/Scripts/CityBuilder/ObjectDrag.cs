using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 _offset;

    void OnMouseDown()
    {
        _offset = transform.position - BuildingSystem.GetMouseWorldPosition();

        Debug.Log($"Building's relocation: {transform.name}");
    }

    void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + _offset;
        transform.position = BuildingSystem.I.SnapCoordinatesToGrid(pos);
    }

    private void OnMouseUp()
    {
        Debug.Log($"{transform.name} relocated to: {transform.position}");
    }
}