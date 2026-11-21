using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingItem : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _createButton;
    
    public void Init(Sprite preview, string name, Action OnClick)
    {
        _previewImage.sprite = preview;
        _nameText.text = name;
        _createButton.onClick.AddListener(() => OnClick?.Invoke());
    }
}