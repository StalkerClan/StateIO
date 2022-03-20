using UnityEngine;

public static class GlobalVariables
{
    public enum Owner
    {
        Player,
        Neutral,
        Enemy,
    }

    public enum UIType
    {
        BlockRaycast, 
        Message,
        MainMenu,
        Gameplay,
        Settings,
        Store,
        Button,
        GameOverScreen,
    }

    public enum UpgradeType
    {
        StartUnits,
        ProduceSpeed,
    }
}