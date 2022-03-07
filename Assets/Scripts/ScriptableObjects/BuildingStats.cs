using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building")]
public class BuildingStats : ScriptableObject
{
    [Header("Name")]
    public new string name;

    [Header("Stats")]
    public ColorSet colorSet;

    public float timeToGenerate;

    public int startFighter;
    public int maxCapacity;

    [Header("Model")]
    public Sprite avatar;
}
