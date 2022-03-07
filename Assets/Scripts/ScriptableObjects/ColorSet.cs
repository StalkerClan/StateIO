using UnityEngine;

[CreateAssetMenu(fileName = "New Color Set", menuName = "Scriptable Objects/Color Set")]
public class ColorSet : ScriptableObject
{
    [Header("Key Color")]
    public Color keyColor;

    [Header("Lerp Colors")]
    public Color firstColor;
    public Color secondColor;
}
