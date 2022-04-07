using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public int Level = 1;
    public int Cost;
    public float Value;
}
