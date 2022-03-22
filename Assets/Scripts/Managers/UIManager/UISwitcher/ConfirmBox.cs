using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBox : MonoBehaviour
{
    public void Confirm()
    {
        LevelManager.Instance.LevelGenerator.UnParentAllLevels();
        GameManager.Instance.SwitchState(GameState.MainMenu);
        LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
    }

    public void Cancel()
    {
        if (UIManager.Instance.LastActiveCanvas is UIGameplay)
        {
            (UIManager.Instance.LastActiveCanvas as UIGameplay).OnPlaying();
        }
    }
}
