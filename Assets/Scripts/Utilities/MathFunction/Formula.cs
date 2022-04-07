using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula 
{
    private static int baseGoldRequired = 50;
    private static int additionFighter = 1;
    private static int baseGoldEarningOffline = 100;
    private static int baseGoldWinning = 200;

    private static float baseProduceSpeed = 1f;

    public static int UpgradeCost(int level)
    {
        return baseGoldRequired * (level - 1) + 95;
    }

    public static int OfflineEarning(int level)
    {
        return baseGoldEarningOffline + 75 * level;
    }

    public static int WinningGoldEarned(int level)
    {
        return baseGoldWinning + 160 * (level - 1);
    }

    public static int LoseGoldEarned(int level)
    {
        return (int) (baseGoldWinning * level * 0.1);
    }

    public static int StartUnits(int startUnits)
    {
        return startUnits += additionFighter; 
    }

    public static float ProduceSpeed(int level)
    {
        float value = baseProduceSpeed + 0.075f * level;
        double rounded = Math.Round(value, 2);
        return ((float) rounded);
    }

    public static int GoldOfflineEarned(DateTime lastTimePlayed, int gold)
    {
        TimeSpan awayTime = DateTime.Now.Subtract(lastTimePlayed);
        double totalSeconds = awayTime.TotalSeconds;
        if (totalSeconds > 43200)
        {
            totalSeconds = 43200;
        }
        float goldEarnedPerSecond = gold / 360f;
        float totalGoldEarned = (float) totalSeconds * goldEarnedPerSecond;
        return Mathf.RoundToInt(totalGoldEarned);
    }
    
    public static string TimeAway(DateTime lastTimePlayed)
    {
        TimeSpan awayTime = DateTime.Now.Subtract(lastTimePlayed);
        double totalSeconds = awayTime.TotalSeconds;
        string formatted;
        if (totalSeconds > 43200)
        {
            formatted = "12:00:00";
        }
        else
        {
            formatted = awayTime.ToString(@"hh\:mm\:ss");
        }
        return formatted;
    }
}
