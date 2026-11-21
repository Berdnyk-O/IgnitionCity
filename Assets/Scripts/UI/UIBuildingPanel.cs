using UnityEngine;

public class UIBuildingPanel : MonoBehaviour
{
    [SerializeField] private BuildingData[] _buildingDatas;
    [SerializeField] private GameObject _buildingPanelContent;
    [SerializeField] private GameObject _roadPanelContent;
    [SerializeField] private UIBuildingItem _UIBuildingItemPrefab;

    [SerializeField] private GameObject _placementSection;

    void Start()
    {
        for (int i = 0; i < _buildingDatas.Length; i++)
        {
            int index = i;
            GameObject parent = _buildingPanelContent;
            if(_buildingDatas[index].Type == BuildingType.Road)
            {
                parent = _roadPanelContent;
            }

            UIBuildingItem building = Instantiate(
                _UIBuildingItemPrefab,
                parent.transform
            );

            building.Init(
                _buildingDatas[index].Preview,
                _buildingDatas[index].Name,
                () => OnCreateBuilding(index)
            );
        }
    }

    private void OnCreateBuilding(int index)
    {
        BuildingSystem.I.InitializateWithObject(_buildingDatas[index]);
        _placementSection.SetActive(true);
    }

    public void OnPlaceBuilding()
    {
        _placementSection.SetActive(false);
        BuildingSystem.I.PlaceObject();
    }

    public void OnRotateBuilding(bool toRight)
    {
        BuildingSystem.I.Rotate(toRight);
    }
}