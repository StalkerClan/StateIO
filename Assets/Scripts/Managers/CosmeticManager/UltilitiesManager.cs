using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltilitiesManager : Singleton<UltilitiesManager>
{
    public List<ColorSet> ColorSets = new List<ColorSet>();

    private void Awake()
    {
        ColorSet[] colorSetList = Resources.LoadAll<ColorSet>("ColorSets/");
        ColorSets = colorSetList.ToList();
        foreach (ColorSet colorSet in ColorSets)
        {
            colorSet.EnemyUsed = false;
        }
    }
}
