using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;

    [SerializeField] private BuildingData _buildingData1;
    [SerializeField] private BuildingData _buildingData2;
    [SerializeField] private BuildingData _buildingData3;
    [SerializeField] private BuildingPreview _previewPrefab;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private BuildingGrid _grid;

    private BuildingPreview _preview;

    void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        if (_preview != null)
        {
            HandlePreview(mousePosition);
        }
        else if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _preview = CreatePreview(_buildingData1, mousePosition);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _preview = CreatePreview(_buildingData2, mousePosition);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _preview = CreatePreview(_buildingData3, mousePosition);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    private void HandlePreview(Vector3 mouseworldPosition)
    {
        _preview.transform.position = mouseworldPosition;
        List<Vector3> buildPositions = _preview.BuildingModel.GetAllBuildingPositions();
        bool canBuild = _grid.CanBuild(buildPositions);

        if (canBuild)
        {
            _preview.transform.position = GetSnappedCenterPosition(buildPositions);
            _preview.ChangeState(BuildingPreviewState.Positive);

            if (Mouse.current.leftButton.isPressed)
            {
                PlaceBuilding(buildPositions);
            }
        }
        else
        {
            _preview.ChangeState(BuildingPreviewState.Negative);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            _preview.Rotate(90);
        }
    }

    //private Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    //{
    //    List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
    //    List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
    //    //float centerX = (xs.Min() + xs.Max()) / 2f + CellSize / 2f;
    //    //float centerZ = (zs.Min() + zs.Max()) / 2f + CellSize / 2f;

    //    float centerX = MathF.Floor(((xs.Min() + xs.Max()) / 2f + CellSize / 2f) / CellSize) * CellSize;
    //    float centerZ = MathF.Floor(((zs.Min() + zs.Max()) / 2f + CellSize / 2f) / CellSize) * CellSize;
    //    return new(centerX, 0, centerZ);
    //}

    private Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions
            .Select(p => Mathf.FloorToInt(p.x / CellSize))
            .ToList();

        List<int> zs = allBuildingPositions
            .Select(p => Mathf.FloorToInt(p.z / CellSize))
            .ToList();

        float centerX = (xs.Min() + xs.Max() + 1) * CellSize / 2f;
        float centerZ = (zs.Min() + zs.Max() + 1) * CellSize / 2f;

        return new Vector3(centerX, 0, centerZ);
    }

    private void PlaceBuilding(List<Vector3> buildingPositions)
    {
        Building building = Instantiate(_buildingPrefab, _preview.transform.position, Quaternion.identity);
        building.SetUp(_preview.Data, _preview.BuildingModel.Rotation);
        _grid.SetBuilding(building, buildingPositions);
        Destroy(_preview.gameObject);
        _preview = null;
    }

    private BuildingPreview CreatePreview(BuildingData data, Vector3 position) 
    {
        BuildingPreview buildingPreview = Instantiate(_previewPrefab, position, Quaternion.identity);
        buildingPreview.SetUp(data);

        return buildingPreview;
    }
}
