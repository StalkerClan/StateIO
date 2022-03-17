using System;
using UnityEngine;

public class MainMenu : BaseGameState
{
    public event Action<BaseGameState> OnFinishedLevel = delegate { };

    public override void EnterState(GameManager gameManager)
    {
        UIManager.Instance.UIController.ShowMainMenu(); 
        ObjectPooler.Instance.DeSpawnAllFighters();
        LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
