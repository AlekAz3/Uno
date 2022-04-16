using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseColor : MonoBehaviour
{
    private GameManeger game;

    private void Awake()
    {
        game = FindObjectOfType<GameManeger>();
    }

    public void ChooseRed()
    {
        game.CardColorState = CardColor.Red;
        Destroy(this.gameObject);
        game.ChangeTurn();
    }

    public void ChooseYellow()
    {
        game.CardColorState = CardColor.Yellow;
        Destroy(this.gameObject);
        game.ChangeTurn();
    }

    public void ChooseGreen()
    {
        game.CardColorState = CardColor.Green;
        Destroy(this.gameObject);
        game.ChangeTurn();
    }

    public void ChooseBlue()
    {
        game.CardColorState = CardColor.Blue;
        Destroy(this.gameObject);
        game.ChangeTurn();
    }

}
