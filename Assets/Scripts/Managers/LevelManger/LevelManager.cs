using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public int LevelID;

    [SerializeField]private Level currentLevel;

    [SerializeField] private int playerBuilding;
    [SerializeField] private int numberOfBuildings;
    private LevelGenerator levelGenerator;

    public int PlayerBuilding { get => playerBuilding; set => playerBuilding = value; }
    public int NumberOfBuildings { get => numberOfBuildings; set => numberOfBuildings = value; }
    public LevelGenerator LevelGenerator { get => levelGenerator; set => levelGenerator = value; }

    private void Awake()
    {
        LevelID = PlayerPrefs.HasKey("LevelID") ? LevelID = PlayerPrefs.GetInt("LevelID") : LevelID = PlayerPrefs.GetInt("LevelID", 1);     
        levelGenerator = FindObjectOfType<LevelGenerator>();
        LoadMap();
    }

    public void LoadMap()
    {
        SetLevel();
        levelGenerator.LoadMap();
    }
    public void SetLevel()
    {
        currentLevel = levelGenerator.CurrentLevel = levelGenerator.ListLevel[LevelID - 1];
        currentLevel.SetLevelStatus(false, true, false);
        playerBuilding = currentLevel.PlayerStartBuildings.Count;
        numberOfBuildings = currentLevel.PlayableBuildings.Count;
    }

    public void CheckWinCondition()
    {
        if (playerBuilding >= numberOfBuildings)
        {
            LevelCompleted();
        } 
        else if (playerBuilding <= 0)
        {
            GameOver();
        }
    }

    public void LevelCompleted()
    {
        ObjectPooler.Instance.DeSpawnAllFighters();
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(true, false, false);
        LevelID++;
        PlayerPrefs.SetInt("LevelID", LevelID);
        SetLevel();
        levelGenerator.LoadLevel(); 
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }

    public void GameOver()
    {
        GameManager.Instance.SwitchState(GameState.GameOver);
    }  
}
