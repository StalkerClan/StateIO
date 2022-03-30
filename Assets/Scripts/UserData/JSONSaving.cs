using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONSaving : Singleton<JSONSaving>
{
    private OwnerStat userStat;
    private UserData userData;

    private float awayTime;

    private string path;
    private string persistentPath;

    public OwnerStat UserStat { get => userStat; set => userStat = value; }
    public UserData UserData { get => userData; set => userData = value; }
    public float AwayTime { get => awayTime; set => awayTime = value; }

    private void Awake()
    {
        SetPaths();
        LoadUserData();
    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void LoadUserData()
    {
        userStat = Resources.Load<OwnerStat>("OwnerStat/UserStat");
        if (!userStat.Initialized)
        {
            userData = new UserData(userStat, 1, 0, 0);
            userStat.Initialized = true;
            SaveData();
        }
        else
        {
            LoadData();
        }
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "UserData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void SaveData()
    {
        userData.LastHourPlayed = DateTime.Now.Hour;
        userData.LastMinutePlayed = DateTime.Now.Minute;
        string json = JsonUtility.ToJson(userData);
        using StreamWriter writer = new StreamWriter(persistentPath);
        writer.Write(json);
        Debug.Log(persistentPath);
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();
        userData = JsonUtility.FromJson<UserData>(json);
        awayTime = CalculateAwayTime();
        Debug.Log(persistentPath);
        Debug.Log(awayTime);
    }

    public float CalculateAwayTime()
    {
        float currentHour = DateTime.Now.Hour;
        float currentMinute = DateTime.Now.Minute;
        float currentTime = currentHour + (currentMinute / 60);
        float lastTime = userData.LastHourPlayed + (userData.LastMinutePlayed / 60);
        return lastTime - currentTime;
    }
}
