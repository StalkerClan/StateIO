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
        Settings,
        Store, 
        Gameplay,
        Blocker,
        GameOverScreen, 
        Button,
    }

    public enum UpgradeType
    {
        StartUnits,
        ProduceSpeed,
    }
}