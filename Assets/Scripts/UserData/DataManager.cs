using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private OwnerStat userStat;

    public OwnerStat UserStat { get => userStat; set => userStat = value; }

    private void Awake()
    {
        LoadUserData();
    }

    public void LoadUserData()
    {
        userStat = Resources.Load<OwnerStat>("OwnerStat/UserStat");
    }
}
