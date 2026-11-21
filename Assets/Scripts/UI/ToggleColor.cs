using UnityEngine;
using UnityEngine.UI;

public class ToggleColor : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _background;
    [SerializeField] private Color _onColor = Color.white;
    [SerializeField] private Color _offColor = Color.gray;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(UpdateColor);
        UpdateColor(_toggle.isOn);
    }

    private void UpdateColor(bool isOn)
    {
        _background.color = isOn ? _onColor : _offColor;
    }
}