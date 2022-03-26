using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : Singleton<CosmeticManager>
{
    public List<ColorSet> ColorSets = new List<ColorSet>();

    private void Awake()
    {
        ColorSet[] colorSetList = Resources.LoadAll<ColorSet>("Cosmetics/ColorSet/");
        ColorSets = colorSetList.ToList();
        foreach (ColorSet colorSet in ColorSets)
        {
            colorSet.EnemyUsed = false;
        }
    }
}
