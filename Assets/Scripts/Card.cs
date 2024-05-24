using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    public bool OnFront = false;

    private int _position = -1;
    
    private SpriteRenderer _fruitRenderer;
    private Animator _animator;
    
    public UnityEvent<string, int> OnCardClick;
    
    private void Start()
    {
        var frontSide = transform.Find("card_front").GetComponent<check_click>();
        
        frontSide.onCardFlipped.AddListener(CardClicked);
    }

    private void Awake()
    {
        _fruitRenderer = transform.Find("card_image").GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetFruitSprite(Sprite fruit)
    {
        _fruitRenderer.sprite = fruit;
    }

    public void SetPosition(int position)
    {
        _position = position;
    }

    public int GetPosition()
    {
        return _position;
    }

    private void CardClicked()
    {
        OnCardClick?.Invoke(_fruitRenderer.sprite.name, _position);
    }

    public string GetFruitName()
    {
        return _fruitRenderer.sprite.name;
    }
    public void Flip()
    {
        if (OnFront)
        {
            _animator.SetTrigger("FlipToBack");
            OnFront = false;
        }
        else
        {
            _animator.SetTrigger("FlipToFront");
            OnFront = true;
        }
    }
}
