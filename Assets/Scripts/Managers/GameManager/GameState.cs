using UnityEngine;

public abstract class GameState
{
    public abstract void EnterState(GameManager gameManager);

    public abstract void UpdateState(GameManager gameManager);
}
