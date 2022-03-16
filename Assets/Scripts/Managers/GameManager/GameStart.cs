using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : BaseGameState
{
    public override void EnterState(GameManager gameManager)
    {

    }

    public override void UpdateState(GameManager gameManager)
    {
        CameraController.Instance.ZoomIn();
    }
}
