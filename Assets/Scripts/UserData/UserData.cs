using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public OwnerStat UserStat;
    public int Level;
    public float LastHourPlayed;
    public float LastMinutePlayed;


    public UserData(OwnerStat ownerStat, int level, float lastHourPlayed, float lastMinutePlayed)
    {
        UserStat = ownerStat;
        Level = level;
        LastHourPlayed = lastHourPlayed;
        LastMinutePlayed = lastMinutePlayed;
    }
}
