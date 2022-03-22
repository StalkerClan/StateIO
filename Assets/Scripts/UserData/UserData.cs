using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData 
{
    public int Level;
    public OwnerStat UserStat;

    public UserData(int level, OwnerStat ownerStat)
    {
        Level = level;
        UserStat = ownerStat;
    }
}
