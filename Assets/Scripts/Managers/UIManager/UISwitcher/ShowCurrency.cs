using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCurrency : ShowOnScreen
{   
    private void Start()
    {
        ShowInfo(LevelManager.Instance.PlayerCurrency);
        SubcribeEvent();
    }

    private void OnDestroy()
    {
        UnsubcribeEvent();
    }

    public override void SubcribeEvent()
    {
        LevelManager.Instance.OnAddingCurrency += ShowInfo;
        UpgradeSystem.Instance.OnBuyingUpgrade += ShowInfo;
    }

    public override void UnsubcribeEvent()
    {
        LevelManager.Instance.OnAddingCurrency -= ShowInfo;
        UpgradeSystem.Instance.OnBuyingUpgrade -= ShowInfo;
    }

    public override void ShowInfo(int info)
    {
        TextInfo.text = info.ToString();
    }
}
