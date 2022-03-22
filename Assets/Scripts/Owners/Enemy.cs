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
        this.ColorSet.EnemyUsed = false;
        newColorSet.EnemyUsed = true;
        base.ChangeColor(newColorSet);
    }
}
