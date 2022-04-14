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

    public GameObject ChooseColor;

    int Turn = 2, TurnTime = 30;

    public TextMeshProUGUI TurnTimeTxt;
    public TextMeshProUGUI TurnTxt;
    public Button TakeCard;
    public Button ChangeTurnButton;
    public Image StartScreen;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultText;



    public List<CardInfo> Enemy1_HandCards = new List<CardInfo>(),
                   Enemy2_HandCards = new List<CardInfo>(),
                   Player_HandCards = new List<CardInfo>(),
                   Deck_Cards = new List<CardInfo>();

    public CardInfo CurrentCardInFied
    {
        get {return Deck_Cards[Deck_Cards.Count - 1];}
        set {Deck_Cards[Deck_Cards.Count - 1] = value;}
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
        get {return Math.Abs(Turn % 3) == 2;}
    }

    public void StartGame()
    {
        StartScreen.gameObject.SetActive(false);
        ResultGO.SetActive(false);
        CurrentGame = new Game();



        Player_HandCards.Clear();
        Enemy1_HandCards.Clear();
        Enemy2_HandCards.Clear();
        Deck_Cards.Clear();

        GiveHandCard(CurrentGame.Enemy1_Cards, Enemy1Hand);

        GiveHandCard(CurrentGame.Enemy2_Cards, Enemy2Hand);

        GiveHandCard(CurrentGame.Player_Cards, PlayerHand);

        EnemyStandCard(Enemy2_HandCards, Enemy2_HandCards[Enemy2_HandCards.Count - 1]);
        UseCardAbility();

        StartCoroutine(TurnFunk());
    }

    void GiveHandCard(List<Card> deck, Transform hand)
    {
        for (int i = 0; i < 4; i++)
        {
            GiveCardToHand(deck, hand);
        }
    }

    public IEnumerator TurnFunk()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        if (IsPlayerTurn)
        {
            while (TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                TurnTxt.text = "Ваш ход";
                yield return new WaitForSeconds(1);
            }
        }
        else if (IsEnemy1Turn)
        {
            while (TurnTime-- > 24)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                TurnTxt.text = "Ход противника 1";
                yield return new WaitForSeconds(1);
                Enemy1Turn();
            }
        }
        else if (IsEnemy2Turn)
        {
            while (TurnTime-- > 22)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                TurnTxt.text = "Ход противника 2";
                yield return new WaitForSeconds(1);
                Enemy2Turn();
                
            }
        }
        ChangeTurn();
    }

    private void Enemy1Turn()
    {

        List<CardInfo> CanPlaceCards = new List<CardInfo>();

        foreach (CardInfo card in Enemy1_HandCards)
        {
            if (CurrentGame.CanStandCardInToField(CurrentCardInFied, card))
                CanPlaceCards.Add(card);
        }


        if (CanPlaceCards.Count != 0)
        {
            EnemyStandCard(Enemy1_HandCards, CanPlaceCards[UnityEngine.Random.Range(0, CanPlaceCards.Count)]);
        }
        else
        {
            Debug.Log("Нечем ходить");
            AddCardToHand(CurrentGame.Enemy1_Cards);
            GiveCardToHand(CurrentGame.Enemy1_Cards, Enemy1Hand);
        }
        
        ChangeTurn();
    }

    void Enemy2Turn()
    {

        List<CardInfo> CanPlaceCards = new List<CardInfo>();

        foreach (CardInfo card in Enemy2_HandCards)
        {
            if (CurrentGame.CanStandCardInToField(CurrentCardInFied, card))
                CanPlaceCards.Add(card);
        }


        if (CanPlaceCards.Count != 0)
            EnemyStandCard(Enemy2_HandCards, CanPlaceCards[UnityEngine.Random.Range(0, CanPlaceCards.Count)]);
        else
        {
            Debug.Log("Нечем ходить 2 ");
            AddCardToHand(CurrentGame.Enemy2_Cards);
            GiveCardToHand(CurrentGame.Enemy2_Cards, Enemy2Hand);
        }
        ChangeTurn();
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
        ColorState();
        StopAllCoroutines();
        CheckForResult();
        UseCardAbility();
        
        TakeCard.interactable = IsPlayerTurn;
        ChangeTurnButton.interactable = false;
        StartCoroutine(TurnFunk());
    }

    public void ColorState()
    {
         CurrentColor.color = CurrentCardInFied.color.color;
    }


    public void UseUsedCards()
    {
        int tempcount = Deck_Cards.Count - 1;
        for (int i = 0; i < tempcount; i++)
        {
            CurrentGame.Deck_cards.Add(Deck_Cards[i].SelfCard);
            Deck_Cards.RemoveAt(i);
            
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
        else
        {
            CurrentGame.Player_Cards.Add(CurrentGame.Deck_cards[0]);
            CurrentGame.Deck_cards.RemoveAt(0);
            GiveCardToHand(CurrentGame.Player_Cards, PlayerHand);
            ChangeTurnButton.interactable = true;
        }
    }

    public void EnemyStandCard(List<CardInfo> Enemy_HandCards, CardInfo card)
    {
        Enemy_HandCards.Remove(card);
        Deck_Cards.Add(card);
        card.ShowCardInfo(card.SelfCard);
        card.transform.SetParent(GameObject.Find("Field").transform);
        card.transform.position = new Vector2(960, 540); 
        //card.transform.position = new Vector2(427, 240);
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
                    break;
                case Action.ChangeColor:
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
        for (int i = 0; i < 4; i++)
        {
            AddCardToHand(cards);
            GiveCardToHand(cards, hand);
        }
    }

    public void AddTwo(List<Card> cards, Transform hand)
    {
        for (int i = 0; i < 2; i++)
        {
            AddCardToHand(cards);
            GiveCardToHand(cards, hand);
        }
    }

    public void Block()
    {
        Turn = Turn + CurrentGame.PlusTurn;
        Turn = Turn + CurrentGame.PlusTurn;
    }

    public void Reverse()
    {
        CurrentGame.PlusTurn = -1;
    }

    public void CheckForResult()
    {
        if (Player_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Вы победили";
            StopAllCoroutines();
        }
        else if(Enemy1_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Противник 1 победил";
            StopAllCoroutines();
        }
        else if(Enemy2_HandCards.Count == 0)
        {
            ResultGO.SetActive(true);
            ResultText.text = "Противник 2 победил";
            StopAllCoroutines();
        }
    }
}
