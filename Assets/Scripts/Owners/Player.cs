using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Owner
{
    public event Action<int> OnUpgradeStartUnits = delegate { };
    public event Action<float> OnUpgradeProduceSpeed = delegate { };

    private void OnDestroy()
    {
        ownerStat.ColorSet.PlayerUsed = true;
    }

    public override void ChangeColor(ColorSet newColorSet)
    {
        ColorSet.PlayerUsed = false;
        if (newColorSet.EnemyUsed)
        {
            newColorSet.EnemyUsed = false;
        }
        ColorSet = newColorSet;
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
