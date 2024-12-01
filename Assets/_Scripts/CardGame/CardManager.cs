using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private Transform CardContainer;
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private List<Sprite> CardSprites;
    [SerializeField]
    private int cardCount = 2;
    [SerializeField]
    private int cardPairingCount = 2;
    private List<int> usedSprites = new List<int>();
    [SerializeField]
    private List<Card> generatedRandomCards = new List<Card>();
    [SerializeField]
    private List<Card> completeCards = new List<Card>();

    public void GenerateCards()
    {
        ClearCardContainer();
        List<int> SelectedSprites = RandomizeSprites(cardCount);
        for (int i = 0; i < cardCount; i++)
        {
            GameObject newCardInstance = Instantiate(CardPrefab, CardContainer);
            Card newCard = newCardInstance.GetComponent<Card>();
            newCard.SetCard("card" + i.ToString(), i.ToString(), CardSprites[SelectedSprites[i]]);
            generatedRandomCards.Add(newCard);
            completeCards.Add(newCard);
        }
        GenerateCardPairs(cardPairingCount);
        ReorganizeCardContainer(cardCount);
    }

    private void GenerateCardPairs(int cardPairCount)
    {
        if (generatedRandomCards.Count <= 0) return;
        for (int i = 0; i < cardPairCount - 1; i++)
        {
            foreach (Card generatedCard in generatedRandomCards)
            {
                Card newCardPair = Instantiate(generatedCard, CardContainer);
                completeCards.Add(newCardPair);
            }
        }
    }

    private void ClearCardContainer()
    {
        generatedRandomCards.Clear();
        completeCards.Clear();
        usedSprites.Clear();
        if (CardContainer.childCount > 0)
        {
            for(int i = 0;i < CardContainer.childCount; i++)
            {
                Destroy(CardContainer.GetChild(i).gameObject);
            }
        }
    }

    private void ReorganizeCardContainer(int cardCount)
    {
        GridLayoutGroup cardContainerGrid = CardContainer.GetComponent<GridLayoutGroup>();
        if(cardCount <= 3)
        {
            cardContainerGrid.constraintCount = cardCount;
        }
        else
        {
            cardContainerGrid.constraintCount = 4;
        }
    }

    private List<int> RandomizeSprites(int count)
    {
        if (count == 0) return null;

        List<int> randomizedSprites = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int randomSprite = Random.Range(0, CardSprites.Count);
            while (randomizedSprites.Contains(randomSprite))
            {
                randomSprite = Random.Range(0, CardSprites.Count);
            }

            randomizedSprites.Add(randomSprite);
        }

        return randomizedSprites;
    }

    public Transform GetCardContainer()
    {
        return CardContainer;
    }

    public int GetCardPairingCount()
    {
        return cardPairingCount;
    }

    public int GetCardCount()
    {
        return cardCount;
    }

    public void SetCardCount(int count)
    {
        cardCount = count;
    }

}
