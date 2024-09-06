using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SuperGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject offerPanel, mainPanel;

    [SerializeField] private SuperGameCard[] cardsObjects;
    [SerializeField] private Sprite[] candySprites;

    private int targetCandyIndex = -1;

    private int[] cardsIndexes;
    private List<int> selectedCandyIndexes;

    private const int selectsCount = 3;

    void Start()
    {
        for (int i = 0; i < cardsObjects.Length; i++)
        {
            int index = i;
            cardsObjects[i].AddClickListener(() => SelectCard(index));
        }
    }

    public void Show()
    {
        targetCandyIndex = 0;
        selectedCandyIndexes = new List<int>();

        gameObject.SetActive(true);

        mainPanel.SetActive(false);
        offerPanel.SetActive(true);
    }

    public void Yes()
    {
        mainPanel.SetActive(true);
        offerPanel.SetActive(false);

        InitializeCards();
    }

    private void Hide()
    {
        offerPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        Hide();
        GameController.Instance.SuperGameCanceled();
    }

    private void InitializeCards()
    {
        targetCandyIndex = UnityEngine.Random.Range(0, candySprites.Length);

        int[] notTargetCandyIndexes = Enumerable.Range(0, candySprites.Length).Where(x => x != targetCandyIndex).ToArray();

        cardsIndexes = new int[cardsObjects.Length];
        for (int i = 0; i < cardsObjects.Length; i++)
        {
            if(i < selectsCount)
            {
                cardsIndexes[i] = targetCandyIndex;
            }
            else
            {
                cardsIndexes[i] = notTargetCandyIndexes.GetRandomElement();
            }
        }

        cardsIndexes = Utility.GetRandomEnumerable(cardsIndexes).ToArray();

        for (int i = 0; i < cardsObjects.Length; i++)
        {
            SuperGameCard cardObject = cardsObjects[i];
            cardObject.SetInteractable(true);

            int candyIndex = cardsIndexes[i];
            cardObject.SetCandySprite(candySprites[candyIndex]);

            cardObject.SetDefault();
        }
    }

    private void SelectCard(int index)
    {
        cardsObjects[index].ShowCandy();

        int candyIndex = cardsIndexes[index];
        selectedCandyIndexes.Add(candyIndex);
        if (selectedCandyIndexes.Count >= selectsCount)
        {
            SetAllCardsInteractable(false);

            foreach (var item in selectedCandyIndexes)
            {
                if (item != targetCandyIndex)
                {
                    InvokeComnplete(false);
                    return;
                }
            }

            InvokeComnplete(true);
        }
    }

    private void InvokeComnplete(bool success)
    {
        this.InvokeRealtime(() => Complete(success), 1.5f);
    }

    private void Complete(bool success)
    {
        GameController.Instance.SuperGameCompleted(success);

        Hide();
    }

    private void Lose()
    {
        GameController.Instance.SuperGameCompleted(true);

        Hide();
    }

    private void SetAllCardsInteractable(bool interactable)
    {
        for (int i = 0; i < cardsObjects.Length; i++)
        {
            cardsObjects[i].SetInteractable(interactable);
        }
    }
}