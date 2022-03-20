using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltilitiesManager : Singleton<UltilitiesManager>
{
    public List<ColorSet> ColorSets = new List<ColorSet>();

    private void Awake()
    {
        ColorSet[] colorSets = Resources.LoadAll<ColorSet>("ColorSets/");
        ColorSets = colorSets.ToList();
    }
}
