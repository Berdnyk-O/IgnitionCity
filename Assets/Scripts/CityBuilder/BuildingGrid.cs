using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private BuildingGridCell[,] _grid;

    void Start()
    {
        _grid = new BuildingGridCell[_width, _height];


        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                _grid[x, y] = new();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        if (BuildingSystem.CellSize <= 0 || _width <= 0 || _height <= 0) return;

        Vector3 origin = transform.position;
        for (int x = 0; x <= _width; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, _height * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0;y <= _height; y++)
        {
            Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
            Vector3 end = origin + new Vector3(_width * BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
            Gizmos.DrawLine(start,end);
        }
    }

    public void SetBuilding(Building building, List<Vector3> allBuildingPositipns)
    {
        foreach (var position in allBuildingPositipns)
        {
            (int x, int y) = WorldToGridPosition(position);
            _grid[x, y].SetBuilding(building);
        }
    }

    public bool CanBuild(List<Vector3> allBuildingPositipns)
    {
        foreach (var position in allBuildingPositipns)
        {
            (int x, int y) = WorldToGridPosition(position);

            if (x < 0 || x>= _width || y < 0 || y >= _height) return false;
            if (!_grid[x,y].IsEmpty()) return false;
        }

        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / BuildingSystem.CellSize);

        return (x, y);
    }
}

public class BuildingGridCell
{
    private Building _building;

    public void SetBuilding(Building building)
    {
        _building = building;
    }

    public bool IsEmpty()
    {
        return _building == null;
    }
}