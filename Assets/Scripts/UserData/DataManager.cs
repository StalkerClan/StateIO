using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserData UserData;
    public Player Player;

    private void Awake()
    {
        LoadGameData();
    }

    public void LoadGameData()
    {
        UserData = JSONSaving.Instance.UserData;
        Player = LevelManager.Instance.LevelGenerator.PlayerData as Player;
    }
}
