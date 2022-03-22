using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public BaseGameState currentState;

    private void Awake()
    {
        currentState = GameState.MainMenu;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseGameState gameState)
    {
        currentState = gameState;
        gameState.EnterState(this);
    }
}
