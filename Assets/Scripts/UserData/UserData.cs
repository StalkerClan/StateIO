using System;

[System.Serializable]
public class UserData
{
    public int Level;
    public OwnerStat UserStat;
    public string LastTimePlayedFormatted;
    public string CurrentPlayTimeFormatted;
    public bool PassedFirstLevel;
    public bool StartCounting;

    public UserData(int level, OwnerStat ownerStat)
    {
        UserStat = ownerStat;
        Level = level;
    }
}
