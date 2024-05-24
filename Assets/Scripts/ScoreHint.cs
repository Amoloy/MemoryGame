using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHint : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Show(int score)
    {
        _text.text = $"Score: {score}";
    }

    public void Hide()
    {
        _text.text = "";
    }
}
