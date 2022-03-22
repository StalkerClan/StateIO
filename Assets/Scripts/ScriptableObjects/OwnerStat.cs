using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building")]
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
    public int Currency;

    [Header("Fighter")]
    public float DefaultMoveSpeed;

    [Header("Model")]
    public Sprite BuildingIcon;
    public Sprite FighterIcon;

    [Header("User Data")]
    public bool Initialized = false;
}
