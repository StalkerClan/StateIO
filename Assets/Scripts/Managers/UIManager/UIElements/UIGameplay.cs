using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplay : UICanvas
{   
    public GameObject PConfirm;
    public GameObject PExit;

    public void OnPlaying()
    {
        PConfirm.SetActive(false);
        PExit.SetActive(true);
        foreach (Level level in LevelManager.Instance.LevelGenerator.ListLevel)
        {
            if (level.LevelStatus.CurrentStatus == LevelStatus.Status.IsPlaying)
            {
                level.transform.parent = this.transform;
            }
        }
    }

    public void OnExitGamePlay()
    {
        PConfirm.SetActive(true);
        PExit.SetActive(false);
    }
}
