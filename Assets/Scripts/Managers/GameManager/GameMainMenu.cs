using System;
using UnityEngine;

public class MainMenu : BaseGameState
{
    public event Action<BaseGameState> OnFinishedLevel = delegate { };

    public override void EnterState(GameManager gameManager)
    {
        UIManager.Instance.CloseUI(GlobalVariables.UIType.Blocker);
        UIManager.Instance.CloseUI(GlobalVariables.UIType.Gameplay);
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.MainMenu) as UIMainMenu).OpenPanel(); 
        ObjectPooler.Instance.DeSpawnAllFighters();
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
