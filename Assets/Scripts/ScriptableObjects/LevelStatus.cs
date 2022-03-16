using UnityEngine;

[CreateAssetMenu(fileName = "New Level Status", menuName = "Scriptable Objects/Level Status")]
public class LevelStatus : ScriptableObject
{
    public bool Completed = false;
    public bool IsPlaying = false;
    public bool Locked = true;
}
