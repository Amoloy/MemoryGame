using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class check_click : MonoBehaviour
{
    public UnityEvent onCardFlipped;

    private void OnMouseDown()
    {
        onCardFlipped?.Invoke();
    }
}
