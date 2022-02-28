using System.Collections;
using UnityEngine;

public static class GlobalVariables
{
    public static Color PLAYER_COLOR;
    public static Color OPPOMENT_COLOR;

    public static string PLAYER_BUILDING_COLOR_HEX = "F04A57";
    public static string NEUTRAL_BUILDING_COLOR_HEX = "FFFFFF";
    public static string OPPONENT_BUILDING_COLOR_HEX = "4555DB";
    public static string NEUTRAL_STATE_COLOR_HEX = "B2B1B1";

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