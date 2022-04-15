using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public Card SelfCard;
    public TextMeshProUGUI Action;
    public Image color;
    public Sprite Back;
    public bool IsWorked = false;
    public bool CanDrag = false;

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        Action.text = card.action.ToString();
        switch (card.color)
        {
            case CardColor.Black:
                color.color = Color.black;
                break;

            case CardColor.Red:
                color.color = Color.red;
                break;

            case CardColor.Blue:
                color.color = Color.blue;
                break;

            case CardColor.Yellow:
                color.color = Color.yellow;
                break;

            case CardColor.Green:
                color.color = Color.green;
                break;
        }
    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        Action.text = "";
        color.color = Color.magenta;
    }


}
