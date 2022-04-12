using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Owner
{
    private void OnDisable()
    {
        ColorSet.EnemyUsed = false;
    }

    public override void ChangeColor(ColorSet newColorSet)
    {
        ColorSet = newColorSet;
        ColorSet.EnemyUsed = true;
        base.ChangeColor(newColorSet);
    }
}
