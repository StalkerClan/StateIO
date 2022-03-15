using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BaseGameState currentState;

    private void Awake()
    {
        currentState = GameState.MainMenu;
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
