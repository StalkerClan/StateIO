using System;
using UnityEngine;

public class MainMenu : BaseGameState
{

    public override void EnterState(GameManager gameManager)
    {
        if (UIManager.Instance.LastActiveCanvas != null)
        {
            if (UIManager.Instance.LastActiveCanvas.Type == GlobalVariables.UIType.Gameplay)
            {
                LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
                ObjectPooler.Instance.DeSpawnAllFighters();
                LevelManager.Instance.LevelGenerator.EnableMap();
                PlayerInput.Instance.IsPlaying = false;
            }
        }    
        
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.MainMenu) as UIMainMenu).OpenPanel(); 
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomOut();
    }
}
  
