using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplay : UICanvas
{
    public void BackToMainMenu()
    {
        LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }
}
