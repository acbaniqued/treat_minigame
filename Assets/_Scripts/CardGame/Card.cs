using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    public string Id;
    private string cardName;
    [SerializeField]
    private Image cardImage;

    public void SetCard(string newId, string newName, Sprite newSprite)
    {
        Id = newId;
        cardName = newName;
        cardImage.sprite = newSprite;
    }
}
