using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem I;

    public GridLayout gridLayout;

    [SerializeField] private Tilemap _mainTilemap;
    [SerializeField] private TileBase _whiteTile;

    private Grid _grid;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private PlaceableObject _objectToPlace;

    void Awake()
    {
        I = this;
        _grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InitializateWithObject(prefab1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InitializateWithObject(prefab2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InitializateWithObject(prefab3);
        }

        if(!_objectToPlace)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced(_objectToPlace))
            {
                _objectToPlace.Place();

                Vector3Int start = gridLayout.WorldToCell(_objectToPlace.GetStartPosition());
                TakeArea(start, _objectToPlace.Size);
            }
            else
            {
                Destroy(_objectToPlace.gameObject);
            }
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hit))
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

    public void InitializateWithObject(GameObject prefab)
    {
        Debug.Log($"Beginning of {prefab} initialization");

        Vector3 position = SnapCoordinatesToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        _objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

        Debug.Log($"{prefab} initialized, pos: {position}");
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