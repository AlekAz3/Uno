using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    public Game CurrentGame;

    public Image CurrentColor;
    public Transform Enemy1Hand, Enemy2Hand, PlayerHand;

    public GameObject CardPref;
    public TextMeshProUGUI Info;

    public GameObject ChooseColor;

    int Turn;

    public TextMeshProUGUI TurnTxt;
    public Button TakeCard;
    public Button ChangeTurnButton;
    public Image StartScreen;

    public CardColor CardColorState;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultText;

    public List<CardInfo> Enemy1_HandCards = new List<CardInfo>(),
                   Enemy2_HandCards = new List<CardInfo>(),
                   Player_HandCards = new List<CardInfo>(),
                   Deck_Cards = new List<CardInfo>();

    public CardInfo CurrentCardInFied
    {
        get { return Deck_Cards[Deck_Cards.Count - 1]; }
        set { Deck_Cards[Deck_Cards.Count - 1] = value; }
    }

    public bool IsPlayerTurn
    {
        get { return Math.Abs(Turn % 3) == 0; }
    }

    public bool IsEnemy1Turn
    {
        get { return Math.Abs(Turn % 3) == 1; }
    }

    public bool IsEnemy2Turn
    {
        get { return Math.Abs(Turn % 3) == 2; }
    }

    public bool IsTwoCardPlayer
    {
        get { return Player_HandCards.Count == 2; }
    }

    public bool UnoRule = false;


    public void StartGame()
    {
        if (CurrentGame != null)
            CurrentGame = null;
        
        StartScreen.gameObject.SetActive(false);
        ResultGO.SetActive(false);

        CurrentGame = new Game();

        Player_HandCards.Clear();
        Enemy1_HandCards.Clear();
        Enemy2_HandCards.Clear();

        Deck_Cards.Clear();

        ClearChildren(PlayerHand);
        ClearChildren(Enemy2Hand);
        ClearChildren(Enemy1Hand);

        GiveHandCard(CurrentGame.Enemy1_Cards, Enemy1Hand);

        GiveHandCard(CurrentGame.Enemy2_Cards, Enemy2Hand);

        GiveHandCard(CurrentGame.Player_Cards, PlayerHand);

        Turn = UnityEngine.Random.Range(0, 3);

        FirstCardStand();

        StartCoroutine(TurnFunk());
    }

    public void ClearChildren(Transform hand)
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[hand.transform.childCount];

        foreach (Transform child in hand.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        foreach (GameObject child in allChildren)
            DestroyImmediate(child.gameObject);
    }

    private void FirstCardStand()
    {
        List<CardInfo> CanPlaceCards = new List<CardInfo>();

        if (IsPlayerTurn)
        {
            foreach (CardInfo card in Player_HandCards)
                if (card.SelfCard.color != CardColor.Black)
                    CanPlaceCards.Add(card);
            StandCard(Player_HandCards, CanPlaceCards[CanPlaceCards.Count - 1]);
        }
        else if (IsEnemy1Turn)
            if (Enemy1_HandCards[Enemy1_HandCards.Count - 1].SelfCard.color == CardColor.Black)
                StandCardWithColor(Enemy1_HandCards, Enemy1_HandCards[Enemy1_HandCards.Count - 1], (CardColor)UnityEngine.Random.Range(0, 4));
            else
                StandCard(Enemy1_HandCards, Enemy1_HandCards[Enemy1_HandCards.Count - 1]);
        else if (IsEnemy2Turn)
            if (Enemy2_HandCards[Enemy2_HandCards.Count - 1].SelfCard.color == CardColor.Black)
                StandCardWithColor(Enemy2_HandCards, Enemy2_HandCards[Enemy2_HandCards.Count - 1], (CardColor)UnityEngine.Random.Range(0, 4));
            else
                StandCard(Enemy2_HandCards, Enemy2_HandCards[Enemy2_HandCards.Count - 1]);
        UseCardAbility();
        ColorState();
    }

    void GiveHandCard(List<Card> deck, Transform hand)
    {
        for (int i = 0; i < 7; i++)
            GiveCardToHand(deck, hand);
    }

    public IEnumerator TurnFunk()
    {
        if (IsPlayerTurn)
        {
            SendMessageToUser(" ");
            TurnTxt.text = "Your turn";
            yield return new WaitForSeconds(1);
        }
        else if (IsEnemy1Turn)
        {
            TurnTxt.text = "Opponent 1 turn";
            yield return new WaitForSeconds(1);
            EnemyTurn(Enemy1_HandCards);
            yield return new WaitForSeconds(1);
            ChangeTurn();
        }
        else if (IsEnemy2Turn)
        {
            TurnTxt.text = "Opponent 2 turn";
            yield return new WaitForSeconds(1);
            EnemyTurn(Enemy2_HandCards);
            yield return new WaitForSeconds(1);
            ChangeTurn();
        }
    }

    private void EnemyTurn(List<CardInfo> HandCards)
    {
        List<CardInfo> CanPlaceCards = new List<CardInfo>();

        foreach (CardInfo card in HandCards)
            if (CurrentGame.CanStandCardInToField(CurrentCardInFied, card, CardColorState))
                CanPlaceCards.Add(card);

        int id = UnityEngine.Random.Range(0, CanPlaceCards.Count);

        if (CanPlaceCards.Count != 0)
            if (CanPlaceCards[id].SelfCard.color == CardColor.Black)
                StandCardWithColor(HandCards, CanPlaceCards[id], (CardColor)UnityEngine.Random.Range(0, 4));
            else
                StandCard(HandCards, CanPlaceCards[id]);
        else
            if (IsEnemy1Turn)
            {
                SendMessageToUser("Enemy1 skip");
                AddCardToHand(CurrentGame.Enemy1_Cards);
                GiveCardToHand(CurrentGame.Enemy1_Cards, Enemy1Hand);
            }
            else if(IsEnemy2Turn)
            {
                SendMessageToUser("Enemy2 skip");
                AddCardToHand(CurrentGame.Enemy2_Cards);
                GiveCardToHand(CurrentGame.Enemy2_Cards, Enemy2Hand);
            }
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        Card card = deck[0];

        GameObject cardGO = Instantiate(CardPref, hand, false);

        if (hand == PlayerHand)
        {
            cardGO.GetComponent<CardInfo>().ShowCardInfo(card);
            cardGO.GetComponent<CardInfo>().CanDrag = true;
            Player_HandCards.Add(cardGO.GetComponent<CardInfo>());
        }
        else if (hand == Enemy1Hand)
        {
            cardGO.GetComponent<CardInfo>().HideCardInfo(card);
            Enemy1_HandCards.Add(cardGO.GetComponent<CardInfo>());
        }
        else
        {
            cardGO.GetComponent<CardInfo>().HideCardInfo(card);
            Enemy2_HandCards.Add(cardGO.GetComponent<CardInfo>());
        }

        deck.RemoveAt(0);
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        CheckForResult();
        CheckUnoRule();
        UseCardAbility();
        ColorState();
        TakeCard.interactable = IsPlayerTurn;
        ChangeTurnButton.interactable = false;
        StartCoroutine(TurnFunk());
    }

    private void CheckUnoRule()
    {
        if (Player_HandCards.Count == 1 && UnoRule == false && UnityEngine.Random.Range(0, 3) == 1 && IsPlayerTurn)
        {
            SendMessageToUser("You didn't say Uno, draw 2 cards");
            GiveNewCardtoPlayer();
            GiveNewCardtoPlayer();
        }
        if (Enemy1_HandCards.Count == 1 && UnityEngine.Random.Range(0, 3) == 1 && IsEnemy1Turn)
        {
            SendMessageToUser("Enemy 1 didn't say Uno, draw 2 cards");

            AddCardToHand(CurrentGame.Enemy1_Cards);
            GiveCardToHand(CurrentGame.Enemy1_Cards, Enemy1Hand);

            AddCardToHand(CurrentGame.Enemy1_Cards);
            GiveCardToHand(CurrentGame.Enemy1_Cards, Enemy1Hand);
        }
        if (Enemy2_HandCards.Count == 1 && UnityEngine.Random.Range(0, 3) == 2 && IsEnemy2Turn)
        {
            SendMessageToUser("Enemy 2 didn't say Uno, draw 2 cards");

            AddCardToHand(CurrentGame.Enemy2_Cards);
            GiveCardToHand(CurrentGame.Enemy2_Cards, Enemy2Hand);

            AddCardToHand(CurrentGame.Enemy2_Cards);
            GiveCardToHand(CurrentGame.Enemy2_Cards, Enemy2Hand);
        }
        UnoRule = false;
    }

    public void SendMessageToUser(string text) => Info.text = text;

    public void ColorState()
    {
        switch (CardColorState)
        {
            case CardColor.Black:
                CurrentColor.color = Color.black;
                break;

            case CardColor.Red:
                CurrentColor.color = Color.red;
                break;

            case CardColor.Blue:
                CurrentColor.color = Color.blue;
                break;

            case CardColor.Yellow:
                CurrentColor.color = Color.yellow;
                break;

            case CardColor.Green:
                CurrentColor.color = Color.green;
                break;
        }
    }

    public void UseUsedCards()
    {
        int tempcount = Deck_Cards.Count - 1;
        for (int i = 0; i < tempcount; i++)
        {
            CurrentGame.Deck_cards.Add(Deck_Cards[0].SelfCard);
            Deck_Cards.RemoveAt(0);
        }
        Game.Shuffle<Card>(CurrentGame.Deck_cards);
    }

    public void AddCardToHand(List<Card> cards)
    {
        if (CurrentGame.Deck_cards.Count == 0)
            UseUsedCards();
        cards.Add(CurrentGame.Deck_cards[0]);
        CurrentGame.Deck_cards.RemoveAt(0);
    }

    public void GiveNewCardtoPlayer()
    {
        if (CurrentGame.Deck_cards.Count == 0)
            UseUsedCards();

        CurrentGame.Player_Cards.Add(CurrentGame.Deck_cards[0]);
        CurrentGame.Deck_cards.RemoveAt(0);
        GiveCardToHand(CurrentGame.Player_Cards, PlayerHand);
        ChangeTurnButton.interactable = true;
        TakeCard.interactable = false;

    }

    public void StandCard(List<CardInfo> HandCards, CardInfo card)
    {
        HandCards.Remove(card);
        Deck_Cards.Add(card);
        card.ShowCardInfo(card.SelfCard);
        card.CanDrag = false;
        CardColorState = card.SelfCard.color;
        card.transform.SetParent(GameObject.Find("Field").transform);
        card.transform.position = new Vector2(960, 540);
    }

    public void StandCardWithColor(List<CardInfo> HandCards, CardInfo card, CardColor color)
    {
        StandCard(HandCards, card);
        CardColorState = color;
    }

    public void UseCardAbility()
    {
        if (!CurrentCardInFied.IsWorked)
        {
            switch (CurrentCardInFied.SelfCard.action)
            {
                case Action.Zero:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.One:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Two:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Three:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Four:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Five:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Six:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Seven:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Eight:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Nine:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.Block:
                    Block();
                    break;
                case Action.Reverse:
                    Reverse();
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.PlusTwo:
                    Turn = Turn + CurrentGame.PlusTurn;
                    if (IsPlayerTurn)
                        AddTwo(CurrentGame.Player_Cards, PlayerHand);
                    else if (IsEnemy1Turn)
                        AddTwo(CurrentGame.Enemy1_Cards, Enemy1Hand);
                    else if (IsEnemy2Turn)
                        AddTwo(CurrentGame.Enemy2_Cards, Enemy2Hand);
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.PlusFour:
                    Turn = Turn + CurrentGame.PlusTurn;
                    if (IsPlayerTurn)
                        AddFour(CurrentGame.Player_Cards, PlayerHand);
                    else if (IsEnemy1Turn)
                        AddFour(CurrentGame.Enemy1_Cards, Enemy1Hand);
                    else if (IsEnemy2Turn)
                        AddFour(CurrentGame.Enemy2_Cards, Enemy2Hand);
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
                case Action.ChangeColor:
                    Turn = Turn + CurrentGame.PlusTurn;
                    break;
            }
            CurrentCardInFied.IsWorked = true;
        }
        else
            Turn = Turn + CurrentGame.PlusTurn;
        CurrentCardInFied.IsWorked = true;
    }

    public void AddFour(List<Card> cards, Transform hand)
    {
        SendMessageToUser("Add Four");
        for (int i = 0; i < 4; i++)
        {
            AddCardToHand(cards);
            GiveCardToHand(cards, hand);
        }
    }

    public void AddTwo(List<Card> cards, Transform hand)
    {
        SendMessageToUser("Add Two");
        for (int i = 0; i < 2; i++)
        {
            AddCardToHand(cards);
            GiveCardToHand(cards, hand);
        }
    }

    public void Block()
    {
        SendMessageToUser("Block");
        Turn = Turn + CurrentGame.PlusTurn;
        Turn = Turn + CurrentGame.PlusTurn;
    }

    public void Reverse()
    {
        SendMessageToUser("Reverse");
        CurrentGame.PlusTurn *= -1;
    }

    public void CheckForResult()
    {
        if (Player_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Win";
            StopAllCoroutines();
        }
        else if (Enemy1_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Lose";
            StopAllCoroutines();
        }
        else if (Enemy2_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Lose";
            StopAllCoroutines();
        }
    }

    public void Quit() => Application.Quit();
}
