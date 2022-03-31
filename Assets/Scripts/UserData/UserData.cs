using System;

[System.Serializable]
public class UserData
{
    public int Level;
    public DateTime lastTimePlayed;
    public OwnerStat UserStat;

    public UserData(int level, OwnerStat ownerStat, DateTime time)
    {
        UserStat = ownerStat;
        Level = level;
        lastTimePlayed = time;
    }
}
