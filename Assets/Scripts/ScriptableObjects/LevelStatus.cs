using UnityEngine;

[CreateAssetMenu(fileName = "New Level Status", menuName = "Scriptable Objects/Level Status")]
public class LevelStatus : ScriptableObject
{
    public enum Status
    {
        Completed,
        IsPlaying,
        Locked,
    }

    public Status CurrentStatus = Status.Locked;
}
