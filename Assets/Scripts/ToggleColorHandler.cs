using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleColorHandler : MonoBehaviour
{
    public Image BoxImage;
    public Color NormalColor;
    public Color SelectedColor;

    public UnityEvent<int> SizeChoosen;

    private Toggle _toggle;
    private Text _text;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _text = GetComponentInChildren<Text>();
    }

    public void OnToggleChanged()
    {
        BoxImage.color = _toggle.isOn ? SelectedColor : NormalColor;

        if (_toggle.isOn)
        {
            int size = Convert.ToInt32(_text.text);
            SizeChoosen?.Invoke(size);
        }
    }
}
