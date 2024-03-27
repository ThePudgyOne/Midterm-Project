using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] GameObject cardBack;
    [SerializeField] private Sprite originalCardBack;
    [SerializeField] private Sprite alternateCardBack;
    [SerializeField] SceneController controller;

    private int _id;
    public int Id
    {
        get { return _id; }
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        //Debug.Log("Testing 1 2 3");
        //cardBack.SetActive(false);

        if (cardBack.activeSelf && controller.canReveal && controller.flippable == true)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
        }

    }

    public void UpdateCardBack(bool useAlternate)
    {
        if (cardBack != null)
        {

            SpriteRenderer backSpriteRenderer = cardBack.GetComponent<SpriteRenderer>();
            if (backSpriteRenderer != null)
            {
                backSpriteRenderer.sprite = useAlternate ? alternateCardBack : originalCardBack;
            }
        }

    }



    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
