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
        Destroy(gameObject);
        game.ChangeTurn();
    }

    public void ChooseYellow()
    {
        game.CardColorState = CardColor.Yellow;
        Destroy(gameObject);
        game.ChangeTurn();
    }

    public void ChooseGreen()
    {
        game.CardColorState = CardColor.Green;
        Destroy(gameObject);
        game.ChangeTurn();
    }

    public void ChooseBlue()
    {
        game.CardColorState = CardColor.Blue;
        Destroy(gameObject);
        game.ChangeTurn();
    }

}
