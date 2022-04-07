using UnityEngine;

[CreateAssetMenu(fileName = "New Owner Stat", menuName = "Scriptable Objects/Owner Stat")]
public class OwnerStat : ScriptableObject
{
    [Header("Name")]
    public new string name;

    [Header("Stats")]
    public ColorSet ColorSet;

    public float ProduceSpeed;
    public float FighterPerTick;

    public int StartFighter;
    public int MaxCapacity;

    [Header("Resources")]
    public int Gold;

    [Header("Fighter")]
    public float DefaultMoveSpeed;

    [Header("Model")]
    public Sprite BuildingIcon;
    public Sprite FighterIcon;

    [Header("User Data")]
    public bool Initialized = false;
}
