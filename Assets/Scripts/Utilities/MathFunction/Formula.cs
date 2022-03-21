using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula 
{
    public static int GoldRequired(int level)
    {
        int baseGoldEarned = 50;
        return baseGoldEarned * (level - 1) + 95;
    }

    public static int GoldEarned(int level)
    {
        return 0;
    }

    public static int StartUnits(int startUnits)
    {
        int addition = 1;
        return startUnits += addition; 
    }

    public static float ProduceSpeed(int level)
    {
        //level = level == 1 ? 2 : level;
        float baseProduceSpeed = 1f;
        float value = baseProduceSpeed + (level) * 0.075f;
        double rounded = Math.Round(value, 2);
        return ((float) rounded);
    }
}
