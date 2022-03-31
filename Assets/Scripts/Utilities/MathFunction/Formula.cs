using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula 
{
    private static int baseGoldRequired = 50;
    private static int additionFighter = 1;
    private static float baseProduceSpeed = 1f;
    private static int goldBase = 200;
    private static int goldEarningOfflineBase = 100;

    public static int GoldRequired(int level)
    {
        return baseGoldRequired * (level - 1) + 95;
    }

    public static int WinningGoldEarned(int level)
    {
        return goldBase + 160 * level;
    }

    public static int LoseGoldEarned(int level)
    {
        return (goldBase + 160 * (level - 1)) / 10;
    }

    public static int StartUnits(int startUnits)
    {
        return startUnits += additionFighter; 
    }

    public static float ProduceSpeed(int level)
    {
        float value = baseProduceSpeed + (level) * 0.075f;
        double rounded = Math.Round(value, 2);
        return ((float) rounded);
    }
}
