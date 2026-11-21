using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BuildingSelectionManager : MonoBehaviour
{
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
            BuildingObject building = hit.collider.GetComponentInParent<BuildingObject>();

            if (building != null)
            {
                _infoPanel.Show(building);
                return;
            }
        }

        _infoPanel.Hide();
    }
}