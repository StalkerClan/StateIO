using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : BaseGameState
{
    public override void EnterState(GameManager gameManager)
    {
        (UIManager.Instance.OpenUI(GlobalVariables.UIType.Gameplay) as UIGameplay).OnPlaying();
        LevelManager.Instance.LevelGenerator.EnableGenerateFighter();
        LevelManager.Instance.LevelGenerator.DisableLevel();
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomIn();
    }
}
