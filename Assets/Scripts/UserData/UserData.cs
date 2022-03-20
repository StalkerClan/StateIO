using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData 
{
    public int Level;
    public ColorSet ColorSet;  
    public float TimeToGenerate;
    public int StartFighter;
    public int MaxCapacity;
    public int Currency;
    public float DefaultMoveSpeed;
    public Sprite BuildingIcon;
    public Sprite FighterIcon;

    public UserData(int level, ColorSet colorSet, float timeToGenerate, int startFighter, int maxCapacity, int currency, float defaultMoveSpeed, Sprite buildingIcon, Sprite fighterIcon)
    {
        Level = level;
        ColorSet = colorSet;
        TimeToGenerate = timeToGenerate;
        StartFighter = startFighter;
        MaxCapacity = maxCapacity;
        Currency = currency;
        DefaultMoveSpeed = defaultMoveSpeed;
        BuildingIcon = buildingIcon;
        FighterIcon = fighterIcon;
    }
}
