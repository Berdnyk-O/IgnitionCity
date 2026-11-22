using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingSelectionManager : MonoBehaviour
{
    [field: SerializeField] public bool IsDeleteMode { get; set; }

    [SerializeField] private Camera _camera;
    [SerializeField] private BuildingInfoPanel _infoPanel;

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (IsDeleteMode)
            {
                PlaceableObject placeable = hit.collider.GetComponentInParent<PlaceableObject>();
                
                if (placeable != null && BuildingSystem.I.IsNoObjectToPlace)
                {
                    BuildingSystem.I.RemoveBuilding(placeable.gameObject);
                }
                
                return;
            }

            BuildingObject building = hit.collider.GetComponentInParent<BuildingObject>();
            
            if (building != null && BuildingSystem.I.IsNoObjectToPlace)
            {
                _infoPanel.Show(building);
                return;
            }
        }

        _infoPanel.Hide();
    }
}