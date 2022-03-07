using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameStartState GameStartState = new GameStartState();
    public GameOverState GameOverState = new GameOverState();
    public FinishedLevelState FinishedLevelState = new FinishedLevelState();

    private GameState currentState;

    private void Awake()
    {
        currentState = GameStartState;
        GameStartState.EnterState(this);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
           
    }

    public void SwitchState(GameState gameState)
    {
        currentState = gameState;
        gameState.EnterState(this);
    }
}
