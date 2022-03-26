using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "Scriptable Objects/Cosmetic")]
public class Cosmetic : ScriptableObject
{
    public Sprite Artwork;
    public bool Equipped = false;
    public bool Unlocked = false;
}
