using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBox : MonoBehaviour
{
    public void Confirm()
    {
        LevelManager.Instance.LevelGenerator.ResetParentAllLevels();
        LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
        ObjectPooler.Instance.DeSpawnAllFighters();
        GameManager.Instance.SwitchState(GameState.MainMenu);     
    }

    public void Cancel()
    {
        if (UIManager.Instance.LastActiveCanvas is UIGameplay)
        {
            (UIManager.Instance.LastActiveCanvas as UIGameplay).OnPlaying();
        }
    }
}
