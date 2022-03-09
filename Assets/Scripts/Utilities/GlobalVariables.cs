using UnityEngine;

public static class GlobalVariables
{
    public static readonly string PLAYER_BUILDING_TAG = "PlayerBuilding";
    public static readonly string NEUTRAL_BUILDING_TAG = "NeutralBuilding";
    public static readonly string OPPONENT_BUILDING_TAG = "OpponentBuilding";

    public enum Fighter
    {
        Player,
        Enemy,
    }

    public enum Owner
    {
        Player,
        Neutral,
        Enemy,
    }
}