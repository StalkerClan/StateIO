using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Owner
{
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
}
