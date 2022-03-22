using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONSaving : Singleton<JSONSaving>
{
    private OwnerStat userStat;
    private UserData userData;

    private string path;
    private string persistentPath;

    public OwnerStat UserStat { get => userStat; set => userStat = value; }
    public UserData UserData { get => userData; set => userData = value; }

    private void Awake()
    {
        SetPaths();
        LoadUserData();
    }

    private void OnDisable()
    {
        SaveData();
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
        Debug.Log("Saving Data at " + persistentPath);
        string json = JsonUtility.ToJson(userData);
        Debug.Log(json);
        using StreamWriter writer = new StreamWriter(persistentPath);
        writer.Write(json);
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();
        userData = JsonUtility.FromJson<UserData>(json);
        Debug.Log(userData.ToString());
    }
}
