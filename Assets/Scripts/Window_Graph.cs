/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;

    private float xSize = 50f;
    private float yMaximum = 100f;
    private float graphHeight;

    private List<int> _data;

    private List<GameObject> elementsHandler;

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        graphHeight = graphContainer.sizeDelta.y;

        elementsHandler = new List<GameObject>();
        gameObjectList = new List<GameObject>();
        
        //List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        
    }

    public void SetData(List<int> data)
    {
        _data = data;
    }

    public void SetHeight(int boardSize)
    {
        yMaximum = boardSize * 2;
    }

    public void Show()
    {
        ShowLattice();
        ShowGraph(_data);
    }

    public void ClearLattice()
    {
        for (int i = 0; i < elementsHandler.Count; i++)
        {
            Destroy(elementsHandler[i]);
        }

        elementsHandler = new List<GameObject>();
        gameObjectList = new List<GameObject>();
    }

    private void ShowLattice()
    {
        for (int i = 0; i < 20; i++) {
            float xPosition = xSize + i * xSize;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = (i+1).ToString();

            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.SetParent(graphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, 0);
            
            elementsHandler.Add(labelX.gameObject);
            elementsHandler.Add(dashX.gameObject);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-25f, normalizedValue * graphHeight + 5f);
            labelY.GetComponent<Text>().text = Mathf.Round(normalizedValue * yMaximum).ToString();

            elementsHandler.Add(labelY.gameObject);
            
            if (i != 0 && i != separatorCount)
            {
                RectTransform dashY = Instantiate(dashTemplateX);
                dashY.SetParent(graphContainer);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(0, normalizedValue * graphHeight);
                
                elementsHandler.Add(dashY.gameObject);
            }
            
        }
    }
    private void ShowGraph(List<int> valueList) {

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject barGameObject = CreateBar(new Vector2(xPosition, yPosition), xSize * 0.7f);
            gameObjectList.Add(barGameObject);
            
            elementsHandler.Add(barGameObject);
        }
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        Color color;
        ColorUtility.TryParseHtmlString($"#0071FF", out color);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(.5f, 0f);
        return gameObject;
    }

}
