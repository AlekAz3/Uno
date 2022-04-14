using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseColor : MonoBehaviour
{
    private GameManeger gameManeger;

    private void Awake()
    {
        gameManeger = FindObjectOfType<GameManeger>();
    }


    public void ChooseRed()
    {
        gameManeger.CurrentCardInFied.SelfCard.color = CardColor.Red;
        AfterChoose();
    }

    public void ChooseYellow()
    {
        gameManeger.CurrentCardInFied.SelfCard.color = CardColor.Yellow;
        AfterChoose();
    }

    public void ChooseGreen()
    {
        gameManeger.CurrentCardInFied.SelfCard.color = CardColor.Green;
        AfterChoose();
    }

    public void ChooseBlue()
    {
        gameManeger.CurrentCardInFied.SelfCard.color = CardColor.Blue;
        AfterChoose();
    }

    public void AfterChoose()
    {
        gameManeger.ColorState();
        gameManeger.TakeCard.interactable = gameManeger.IsPlayerTurn;
        gameManeger.StartCoroutine(gameManeger.TurnFunk());
        Destroy(this);
    }

}
