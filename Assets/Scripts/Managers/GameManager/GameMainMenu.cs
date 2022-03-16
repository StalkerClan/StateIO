using System;
using UnityEngine;

public class MainMenu : BaseGameState
{
    public event Action<BaseGameState> OnFinishedLevel = delegate { };

    public override void EnterState(GameManager gameManager)
    {
        
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
