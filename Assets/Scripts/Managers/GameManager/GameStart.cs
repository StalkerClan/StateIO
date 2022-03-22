using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : BaseGameState
{
    public override void EnterState(GameManager gameManager)
    {
        UIManager.Instance.CloseUI(GlobalVariables.UIType.MainMenu);
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.Blocker) as UIBlocker).OnBlockingMap();
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.Gameplay) as UIGameplay).OnPlaying();
        LevelManager.Instance.LevelGenerator.EnableGenerateFighter();
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomIn();
    }
}
