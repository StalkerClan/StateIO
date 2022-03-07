using UnityEngine;

public class GameOverState : GameState
{
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log("GameOver");
    }

    public override void UpdateState(GameManager gameManager)
    {
        
    }
}
