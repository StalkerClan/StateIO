using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : Singleton<ColorController>
{
    [System.Serializable]
    public class ColorPicker
    {
        public ColorSet ColorSet;
    }

    private List<ColorPicker> colorList;

    public List<ColorPicker> ColorList { get => colorList; set => colorList = value; }
}
