using System;
using UnityEngine;

public class MainMenu : BaseGameState
{
    public event Action<BaseGameState> OnFinishedLevel = delegate { };

    public override void EnterState(GameManager gameManager)
    {
        if (UIManager.Instance.LastActiveCanvas != null)
        {
            if (UIManager.Instance.LastActiveCanvas.Type == GlobalVariables.UIType.Gameplay)
            {
                LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
                ObjectPooler.Instance.DeSpawnAllFighters();
                LevelManager.Instance.LevelGenerator.EnableMap();
            }
        }    
        
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.MainMenu) as UIMainMenu).OpenPanel(); 
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
