using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public CardColor color;
    public Action action;

    public Card(CardColor color, Action action)
    {
        this.color = color;
        this.action = action;
    }
}


public enum CardColor
{
    Red,
    Yellow,
    Green,
    Blue,
    Black
}

public enum Action
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Block,
    Reverse,
    PlusTwo,
    PlusFour,
    ChangeColor
}

public class CardManeger : MonoBehaviour
{
    public static List<Card> GetAllCards()
    {
        List<Card> AllCards = new List<Card>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 15; j++)
                if (j >= 13)
                    AllCards.Add(new Card(CardColor.Black, (Action)j));
                else
                    AllCards.Add(new Card((CardColor)i, (Action)j));

            for (int j = 1; j < 13; j++)
                AllCards.Add(new Card((CardColor)i, (Action)j));
        }
        return AllCards;
    }
}
