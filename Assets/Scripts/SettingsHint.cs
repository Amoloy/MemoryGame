using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SettingsHint : MonoBehaviour
{
    public float Delay = 3.0f;
    
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Show(string Message)
    {
        _text.text = Message;

        StartCoroutine(HideAfterDelay());
    }

    public void HideText()
    {
        _text.text = "";
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(Delay);

        HideText();
    }
}
