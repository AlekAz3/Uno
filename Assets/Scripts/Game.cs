using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game
{
    public List<Card> Player_Cards, Enemy1_Cards, Enemy2_Cards, Used_Cards, Deck_cards;
    public int PlusTurn = 1;

    public Game()
    {
        DeckCardShuffle();
        Enemy1_Cards = GiveCardToHand();
        Enemy2_Cards = GiveCardToHand();
        Player_Cards = GiveCardToHand();
    }

    public List<Card> GiveCardToHand()
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < 4; i++)
        {
            cards.Add(Deck_cards[0]);
            Deck_cards.RemoveAt(0);
        }
        return cards;
    }

    public void DeckCardShuffle()
    {
        Deck_cards = AllCard.AllCards;
        Shuffle<Card>(Deck_cards);
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            T tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }

    public bool CanStandCardInToField(CardInfo CardField, CardInfo CurrentCard)
    {
        if (CardField.SelfCard.color == CurrentCard.SelfCard.color || CardField.SelfCard.action == CurrentCard.SelfCard.action)
        {
            return true;
        }
        else
            return false;
    }

}