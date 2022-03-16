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
        MainMenu,
        Settings,
        Store,
        GameUI,
        Gameplay,
        GameOverScreen
    }
}