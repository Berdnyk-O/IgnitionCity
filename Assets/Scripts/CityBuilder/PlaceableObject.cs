using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    
    private Vector3[] _vertices;


    void Start()
    {
        GetColliderVertexPositions();
        CalculateSizeInCalls();
    }

    public void RotateRight()
    {
        Rotate(true);
    }

    public void RotateLeft()
    {
        Rotate(false);
    }

    private void Rotate(bool toRight)
    {
        int side = toRight ? 1 : -1;

        transform.Rotate(0, side * 90, 0);
        Size = new Vector3Int(Size.y, Size.x, 1);

        Vector3[] vertices = new Vector3[_vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            int index = (i + side + _vertices.Length) % _vertices.Length;
            vertices[i] = _vertices[index];
        }

        _vertices = vertices;
    }

    private void GetColliderVertexPositions()
    {
        BoxCollider b = GetComponent<BoxCollider>();
        _vertices = new Vector3[4];
        _vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        _vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        _vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        _vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;

        Debug.Log($"Collider vertex positions: {_vertices[0]} {_vertices[1]} {_vertices[2]} {_vertices[3]}");
    }

    private void CalculateSizeInCalls()
    {   
        Vector3Int[] vertices = new Vector3Int[_vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPosition = transform.TransformPoint(_vertices[i]);
            vertices[i] = BuildingSystem.I.gridLayout.WorldToCell(worldPosition);
        }

        Size = new Vector3Int(
               Mathf.Abs((vertices[0] - vertices[1]).x),
               Mathf.Abs((vertices[0] - vertices[3]).y),
               1);

        Debug.Log($"Bulding size: {Size}");
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(_vertices[0]);
    }

    public virtual void Place()
    {
        ObjectDrag drag = GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;

        Debug.Log("<color=green>Bulding Placed</color>");
    }
}