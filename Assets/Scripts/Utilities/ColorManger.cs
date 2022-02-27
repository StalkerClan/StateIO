using UnityEngine;

public class ColorManger 
{
    public Color playerColor;
    public Color OpponentColor;

    public void SetColor()
    {
        GlobalVariables.PLAYER_COLOR = playerColor;
    }
}
