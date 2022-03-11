using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : BaseGameState
{
    public override void EnterState(GameManager gameManager)
    {
        UIManager.Instance.HideUI(UIManager.Instance.UIController.MainMenu);
        UIManager.Instance.ShowUI(UIManager.Instance.UIController.Gameplay);
    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomIn();
    }
}
