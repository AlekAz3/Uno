using UnityEngine;
using UnityEngine.UI;

public class CardMove : MonoBehaviour
{
    public Button ChangeTurnBtn;
    private bool IsDragging = false;
    private bool IsOverDropZone = false;
    public bool IsDragble = true;
    private GameObject dropZone;
    GameManeger game;
    
    private Vector2 startPosition;


    private void Awake()
    {
        game = FindObjectOfType<GameManeger>();
    }

    void Update()
    {
        if (IsDragging && game.IsPlayerTurn)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (game.IsPlayerTurn)
        {
            IsOverDropZone = true;
            dropZone = collision.gameObject;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (game.IsPlayerTurn)
        {
            IsOverDropZone = false;
            dropZone = null;
        }
        
    }

    public void BeginDrag()
    {
        if (IsDragble && game.IsPlayerTurn)
        {
            startPosition = transform.position;
            IsDragging = true;
        }
    }

    public void EndDrag()
    {
        IsDragging = false;

        if (IsOverDropZone && game.IsPlayerTurn && game.CurrentGame.CanStandCardInToField(game.CurrentCardInFied, this.GetComponent<CardInfo>()))
        {
            game.Player_HandCards.Remove(this.GetComponent<CardInfo>());
            game.Deck_Cards.Add(this.GetComponent<CardInfo>());
           // game.CurrentGame.Used_Cards.Add(game.CurrentCardInFied.SelfCard);


            transform.SetParent(GameObject.Find("Field").transform);
            transform.position = new Vector2(960, 540);
            //transform.position = new Vector2(427, 240);
            IsDragble = false;

            game.ChangeTurn();
        }
        else if(game.IsPlayerTurn)
            transform.position = startPosition;
    }
}
