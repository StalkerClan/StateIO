using System;
using UnityEngine;

public class MainMenu : BaseGameState
{
    public event Action<BaseGameState> OnFinishedLevel = delegate { };

    public override void EnterState(GameManager gameManager)
    {
        UIManager.Instance.ShowUI(UIManager.Instance.UIController.MainMenu);
        UIManager.Instance.HideUI(UIManager.Instance.UIController.Gameplay);
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
