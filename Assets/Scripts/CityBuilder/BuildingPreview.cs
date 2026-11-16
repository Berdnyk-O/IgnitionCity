using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public BuildingPreviewState State { get; private set; } = BuildingPreviewState.Negative;
    public BuildingData Data { get; private set; }
    public BuildingModel BuildingModel { get; private set; }

    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeMaterial;

    private List<Renderer> _renderers = new();
    private List<Collider> _colliders = new();

    public void SetUp(BuildingData data)
    {
        Data = data;
        BuildingModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);

        _renderers.AddRange(BuildingModel.GetComponentsInChildren<Renderer>());
        _colliders.AddRange(BuildingModel.GetComponentsInChildren<Collider>());

        foreach (Collider collider in _colliders)
        {
            collider.enabled = false;
        }

        SetPreviewMaterial(State);
    }

    public void ChangeState(BuildingPreviewState newState)
    {
        if (newState == State) return;

        State = newState;
        SetPreviewMaterial(State);
    }

    public void Rotate(int rotationStep)
    {
        BuildingModel.Rotate(rotationStep);
    }

    private void SetPreviewMaterial(BuildingPreviewState newState)
    {
        Material previewMat = newState == BuildingPreviewState.Positive ? _positiveMaterial : _negativeMaterial;

        foreach (Renderer renderer in _renderers)
        {
            Material[] materials = new Material[renderer.materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMat;
            }

            renderer.materials = materials;
        }
    }
}
