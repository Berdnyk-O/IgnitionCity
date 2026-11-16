using UnityEngine;

public class Building : MonoBehaviour
{
    public string Description => _data.Description;

    private BuildingModel _model;
    private BuildingData _data;

    public void SetUp(BuildingData data, float rotation)
    {
        _data = data;
        _model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        _model.Rotate(rotation);
    }
}
