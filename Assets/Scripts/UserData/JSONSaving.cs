using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONSaving : Singleton<JSONSaving>
{
    public DateTime LastTimePlayed;
    public DateTime CurrentTimePlay;
    [SerializeField] private OwnerStat userStat;
    [SerializeField] private UserData userData;

    private string path;
    private string persistentPath;

    public OwnerStat UserStat { get => userStat; set => userStat = value; }
    public UserData UserData { get => userData; set => userData = value; }


    private void Awake()
    {
        SetPaths();
        LoadUserData();
    }

    private void OnDestroy()
    {
        SaveBeforeExit();
        SaveData();
    }

    public void SaveBeforeExit()
    {
        if (userData.Level > 1 && !userData.PassedFirstLevel)
        {
            userData.PassedFirstLevel = true;
        } 

        if (userData.Level > 1 && userData.StartCounting)
        {
            UserData.LastTimePlayedFormatted = userData.CurrentPlayTimeFormatted;
        }
    }

    private void LoadUserData()
    {
        userStat = Resources.Load<OwnerStat>("OwnerStat/UserStat");
        if (!userStat.Initialized)
        {
            userData = new UserData(1, userStat);
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
        if (userData.Level > 1 && userData.PassedFirstLevel)
        {
            LastTimePlayed = DateTime.ParseExact(userData.LastTimePlayedFormatted, 
                "dd/MM/yyyy HH:mm:ss", 
                System.Globalization.CultureInfo.InvariantCulture); 
            userData.CurrentPlayTimeFormatted = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        Debug.Log(persistentPath);
    }
}
