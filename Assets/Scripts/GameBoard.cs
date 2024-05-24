using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    public Sprite[] Fruits;
    public GameObject CardPref;

    //------------board settings
    public Vector3 startPos = new Vector3(-8f, 3f, 0f);
    public bool cardsOnFront = false;
    private Quaternion _startRotation;
    
    public float xOffset = 4f;
    public float yOffset = -3.5f;
    public int cardsCount = 8;
    public float CardScale = 1;
    public int cardsInRow = 5;
    public int cardsSameFruit = 2;
    private int _cardsPicked = 0;

    private GameController _gameController;
    private List<GameObject> _cards;
    private List<Card> _choosenCards;

    public UnityEvent OnMatchedCards;
    public UnityEvent OnNotMatchedCards;
    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _cards = new List<GameObject>();
        _choosenCards = new List<Card>();

        _startRotation = cardsOnFront ? new Quaternion(0, 0, 0, 0) : new Quaternion(0, 180, 0, 0);

        xOffset *= CardScale;
        yOffset *= CardScale;
        _cardsPicked = cardsSameFruit;
    }

    private void Start()
    {
        ShuffleFruitSprites();
        GenerateBoard();
        CardsSubscribe();
        GameControllerSubscribe();
    }

    private void ShuffleFruitSprites()
    {
        for (int i = Fruits.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (Fruits[i], Fruits[randomIndex]) = (Fruits[randomIndex], Fruits[i]);
        }
    }

    private void ShuffleCards(List<GameObject>cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }
    }

    private void SetCardsListPositions(List<GameObject>cards)
    {
        for (int i = 0; i < cardsCount; i++)
        {
            cards[i].GetComponent<Card>().SetPosition(i);
        }
    }
    
    private void GenerateBoard()
    {
        for (int i = 0, j = 0; i < cardsCount; i++)
        {
            GameObject card = Instantiate(CardPref, startPos, _startRotation);
            _cards.Add(card);
            SetUpCard(i, j);

            if ((i + 1) % 2 == 0 && i != 0)
            {
                j++;
            }
        }
        
        ShuffleCards(_cards);
        SetCardsListPositions(_cards);
        
        LocateCards(_cards);
    }

    private void SetUpCard(int indexCard, int indexFruit)
    {
        GameObject card = _cards[indexCard];

        card.GetComponent<Card>().OnFront = cardsOnFront;
        
        card.GetComponent<Card>().SetFruitSprite(Fruits[indexFruit]);
        card.transform.localScale *= CardScale;
    }

    private void LocateCards(List<GameObject>cards)
    {
        Vector3 pos = new Vector3(startPos.x, startPos.y, startPos.z);
        
        for (int i = 0; i < cardsCount; i++)
        {
            _cards[i].transform.position = pos;

            if ((i+1) % cardsInRow == 0 && i != 0)
            {
                pos.x = startPos.x;
                pos.y += yOffset;
            }
            else
            {
                pos.x += xOffset;
            }
        }
    }

    private IEnumerator coroutineShowAllCards(float delay)
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (GameObject card in _cards)
        {
            card.GetComponent<Card>().Flip();
        }
        
        yield return new WaitForSeconds(delay + 1f);
        
        foreach (GameObject card in _cards)
        {
            card.GetComponent<Card>().Flip();
        }
        
        yield return new WaitForSeconds(1f);
        _cardsPicked = 0;
    }
    private void ShowAllCards(float delay)
    {
        _cardsPicked = cardsSameFruit;

        StartCoroutine(coroutineShowAllCards(delay));
    }

    private void GameControllerSubscribe()
    {
        _gameController.ShowCards.AddListener(ShowAllCards);
    }
    private void CardsSubscribe()
    {
        Card[] cards = FindObjectsOfType<Card>();

        foreach (var card in cards)
        {
            card.OnCardClick.AddListener(SomeCardClicked);
        }
    }

    private bool IsChosenSamePosition(int position)
    {
        for (int i = 0; i < _choosenCards.Count; i++)
        {
            if (_choosenCards[i].GetPosition() == position)
            {
                return true;
            }
        }

        return false;
    }

    private void SomeCardClicked(string name, int position)
    {
        if (_cardsPicked == cardsSameFruit || (_choosenCards.Count != 0 && IsChosenSamePosition(position)))
        {
            return;
        }
        
        _cards[position].GetComponent<Card>().Flip();
        _choosenCards.Add(_cards[position].GetComponent<Card>());
        _cardsPicked++;

        if (_cardsPicked == 1)
        {
            return;
        }
        
        if (name != _choosenCards[0].GetFruitName())
        {
            StartCoroutine(coroutineHideNotMatched());
        }
        else if (_choosenCards.Count == cardsSameFruit)
        {
            StartCoroutine(coroutineHideMatched());
        }
    }

    private IEnumerator coroutineHideNotMatched()
    {
        yield return new WaitForSeconds(1f);
        
        foreach (Card card in _choosenCards)
        {
            card.Flip();
        }
        _choosenCards.Clear();

        OnNotMatchedCards?.Invoke();
        
        yield return new WaitForSeconds(1f);

        _cardsPicked = 0;
    }

    private IEnumerator coroutineHideMatched()
    {
        yield return new WaitForSeconds(1f);
        
        foreach (Card card in _choosenCards)
        {
            card.gameObject.SetActive(false);
        }
        _choosenCards.Clear();
        
        OnMatchedCards?.Invoke();
        
        _cardsPicked = 0;
    }

    private void OnDestroy()
    {
        foreach (GameObject card in _cards)
        {
            Destroy(card);
        }
    }
}
