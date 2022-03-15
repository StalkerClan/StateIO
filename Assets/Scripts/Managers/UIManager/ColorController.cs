using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : Singleton<ColorController>
{
    [SerializeField] private List<ColorSet> colorSets;

    public List<ColorSet> ColorSets { get => colorSets; set => colorSets = value; }
}
