using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlocker : UICanvas
{
    public void OnBlockingMap()
    {
        foreach (Level level in LevelManager.Instance.LevelGenerator.ListLevel)
        {
            if (level.LevelStatus.CurrentStatus != LevelStatus.Status.IsPlaying)
            {
                level.transform.parent = this.transform;
            }
        }
    }
}
