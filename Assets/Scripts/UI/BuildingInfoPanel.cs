using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Toggle _fireToggle;

    private BuildingObject _currentBuilding;

    public void Show(BuildingObject building)
    {
        gameObject.SetActive(true);

        _currentBuilding = building;

        _name.text = building.Data.Name;
        _fireToggle.isOn = building.IsOnFire;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnFireToggleChanged(bool value)
    {
        _currentBuilding.IsOnFire = value;
    }
}