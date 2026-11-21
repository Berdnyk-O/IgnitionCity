using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem I;

    public GridLayout gridLayout;

    [SerializeField] private Tilemap _mainTilemap;
    [SerializeField] private TileBase _whiteTile;

    private Grid _grid;

    private PlaceableObject _objectToPlace;
    private BuildingData _placeableObjectData;

    void Awake()
    {
        I = this;
        _grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    void Update()
    {        
        if (!_objectToPlace)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlaceObject();
        }
    }

    public void Rotate(bool toRight)
    {
        if (toRight)
        {
            _objectToPlace.RotateRight();
        }
        else
        {
            _objectToPlace.RotateLeft();
        }
    }    

    public void PlaceObject()
    {
        if (CanBePlaced(_objectToPlace))
        {
            _objectToPlace.Place();
            
            Vector3Int start = gridLayout.WorldToCell(_objectToPlace.GetStartPosition());
            TakeArea(start, _objectToPlace.Size);

            if(_placeableObjectData.Type == BuildingType.Building)
            {
                BuildingObject building = _objectToPlace.gameObject.AddComponent<BuildingObject>();
                building.Init(_placeableObjectData);
            }

            _objectToPlace = null;
        }
        else
        {
            Destroy(_objectToPlace.gameObject);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinatesToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);

        return position;
    }

    public void InitializateWithObject(BuildingData data)
    {
        if (_objectToPlace != null && !_objectToPlace.Placed) 
        {
            Destroy(_objectToPlace.gameObject);
        }

        Debug.Log($"Beginning of {data.Model} initialization");

        Vector3 position = SnapCoordinatesToGrid(Vector3.zero);

        GameObject obj = Instantiate(data.Model, position, Quaternion.identity);
        _objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

        _placeableObjectData = data;

        Debug.Log($"{data.Model} initialized, pos: {position}");
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[(area.size.x+1)  * (area.size.y +1) * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);

            counter++;
        }
        
        return array;
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(_objectToPlace.GetStartPosition());
        area.size = new Vector3Int(
            placeableObject.Size.x+1, 
            placeableObject.Size.y+1,
            placeableObject.Size.z);

        TileBase[] baseArrey = GetTilesBlock(area, _mainTilemap);

        foreach (var b in baseArrey)
        {
            if (b == _whiteTile)
            {
                Debug.Log("<color=red>Buinding cannot be placed</color>");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        _mainTilemap.BoxFill(start, _whiteTile,
            start.x, start.y,
            start.x + size.x, start.y + size.y);
    }
}