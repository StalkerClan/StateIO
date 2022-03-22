using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowLevel : ShowOnScreen
{
    private static string strLevel = "LEVEL ";

    private void Start()
    {
        ShowInfo(LevelManager.Instance.LevelID);
        SubcribeEvent();
    }

    private void OnDestroy()
    {
        UnsubcribeEvent();
    }

    public override void SubcribeEvent()
    {
        LevelManager.Instance.OnShowingLevelID += ShowInfo;
    }
    public override void UnsubcribeEvent()
    {
        LevelManager.Instance.OnShowingLevelID -= ShowInfo;
    }

    public override void ShowInfo(int info)
    {
        TextInfo.text = strLevel + info.ToString();
    }
}
