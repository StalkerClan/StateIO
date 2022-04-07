using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Owner
{
    public event Action<int> OnUpgradeStartUnits = delegate { };
    public event Action<float> OnUpgradeProduceSpeed = delegate { };

    private void OnDisable()
    {
        OwnerStat.ColorSet = ColorSet;
    }

    public override void ChangeColor(ColorSet newColorSet)
    {
        this.ColorSet.PlayerUsed = false;
        newColorSet.PlayerUsed = true;
        base.ChangeColor(newColorSet);
    }

    public void UpgradeStartUnits(int value)
    {
        OwnerStat.StartFighter = value;
        OnUpgradeStartUnits?.Invoke(value);
    }

    public void UpgradeProduceSpeed(float value)
    {
        OwnerStat.ProduceSpeed = value;
        OnUpgradeProduceSpeed?.Invoke(value);
    }

    public void UpgradeOfflineEarning(int value)
    {
        //OwnerStat.BaseGoldOffineEarned = value;
    }
}
