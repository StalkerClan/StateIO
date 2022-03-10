using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building")]
public class OwnerStat : ScriptableObject
{
    [Header("Name")]
    public new string name;

    [Header("Stats")]
    public ColorSet colorSet;

    public float timeToGenerate;

    public int startFighter;
    public int maxCapacity;

    [Header("Resources")]
    public int currency;

    [Header("Fighter")]
    public float defaultMoveSpeed;

    [Header("Model")]
    public Sprite buildingIcon;
    public Sprite fighterIcon;
}
