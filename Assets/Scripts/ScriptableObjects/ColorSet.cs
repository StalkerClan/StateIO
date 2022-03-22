using UnityEngine;

[CreateAssetMenu(fileName = "New Color Set", menuName = "Scriptable Objects/Color Set")]
public class ColorSet : ScriptableObject
{
    [Header("Key Color")]
    public Color KeyColor;

    [Header("Lerp Colors")]
    public Color FirstColor;
    public Color SecondColor;

    [Header("Status")]
    public bool PlayerUsed = false;
    public bool EnemyUsed = false;
    public bool Locked = true;
}
