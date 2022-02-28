using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities 
{
    //delegate void ChangeColor(string ownerColor, SpriteRenderer spriteRenderer);

    private static readonly Dictionary<float, WaitForSeconds> WFSDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (WFSDictionary.TryGetValue(time, out var wait)) return wait;

        WFSDictionary[time] = new WaitForSeconds(time);
        return WFSDictionary[time];
    }

    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
        byte a = 255; //assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public static Color HexToColorWithAlpha(string hex, byte alpha)
    {
        hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
        byte a = alpha; //assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public static void SetStateColor(GlobalVariables.Building owner, SpriteRenderer spriteRenderer)
    {
        switch (owner)
        {
            case GlobalVariables.Building.Player:
                ChangeStateColor(GlobalVariables.PLAYER_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            case GlobalVariables.Building.Neutral:
                ChangeStateColor(GlobalVariables.NEUTRAL_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            case GlobalVariables.Building.Opponent:
                ChangeStateColor(GlobalVariables.OPPONENT_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            default:
                break;
        }
    }

    private static void ChangeStateColor(string ownerColor, SpriteRenderer spriteRenderer)
    {
        Color stateColor = Utilities.HexToColorWithAlpha(ownerColor, 128);
        spriteRenderer.color = stateColor;
    }

    public static void SetBuildingColor(GlobalVariables.Building owner, SpriteRenderer spriteRenderer)
    {
        switch (owner)
        {
            case GlobalVariables.Building.Player:
                ChangeBuildingColor(GlobalVariables.PLAYER_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            case GlobalVariables.Building.Neutral:
                ChangeBuildingColor(GlobalVariables.NEUTRAL_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            case GlobalVariables.Building.Opponent:
                ChangeBuildingColor(GlobalVariables.OPPONENT_BUILDING_COLOR_HEX, spriteRenderer);
                break;
            default:
                break;
        }
    }

    private static void ChangeBuildingColor(string ownerColor, SpriteRenderer spriteRenderer)
    {
        Color stateColor = Utilities.HexToColor(ownerColor);
        spriteRenderer.color = stateColor;
    }
}
