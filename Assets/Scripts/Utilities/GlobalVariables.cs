using UnityEngine;

public static class GlobalVariables
{
    public static Color PLAYER_COLOR;
    public static Color OPPOMENT_COLOR;
    public static readonly string PLAYER_BUILDING_TAG = "PlayerBuilding";
    public static readonly string NEUTRAL_BUILDING_TAG = "NeutralBuilding";
    public static readonly string OPPONENT_BUILDING_TAG = "OpponentBuilding";

    public enum Building
    {
        Player,
        Neutral,
        Opponent,
    }

    public enum Fighter
    {
        Player,
        Opponent,
    }

    public enum Owner
    {
        Player,
        None,
        Opponent,
    }
}