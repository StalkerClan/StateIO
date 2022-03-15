using UnityEngine;

[CreateAssetMenu(fileName = "New Level Status", menuName = "Scriptable Objects/Level Status")]
public class LevelStatus : ScriptableObject
{
    public bool Completed;
    public bool IsPlaying;
    public bool Locked = true;
}
