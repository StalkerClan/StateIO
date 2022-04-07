using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISubcriber
{
    public event Action<string> OnShowingLevelID = delegate { };
    public event Action<string> OnShowingGold = delegate { };

    private LevelGenerator levelGenerator;
    public Level currentLevel;
    public Player Player;
    
    public UserData UserData;
    public int PlayerGold;

    public int GoldEarned;

    public bool PlayerWon;
    public bool PlayerLose;

    public LevelGenerator LevelGenerator { get => levelGenerator; set => levelGenerator = value; }

    private void Awake()
    {
        UserData = JSONSaving.Instance.UserData;
        levelGenerator = FindObjectOfType<LevelGenerator>();
        SubcribeEvent();
        LoadMap();
        Player = levelGenerator.PlayerData as Player;
        PlayerGold = Player.OwnerStat.Gold;
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    public void SubcribeEvent()
    {
        levelGenerator.OnEnemiesOutOfBuildings += LevelCompleted;
        levelGenerator.OnPlayerOutOfBuildings += GameOver;
    }

    public void UnsubcribeEvent()
    {
        levelGenerator.OnEnemiesOutOfBuildings -= LevelCompleted;
        levelGenerator.OnPlayerOutOfBuildings -= GameOver;
    }

    public void LoadMap()
    {
        SetLevel();
        levelGenerator.LoadMap();
    }
    public void SetLevel()
    {
        currentLevel = levelGenerator.CurrentLevel = levelGenerator.ListLevel[UserData.Level - 1];
        currentLevel.SetLevelStatus(LevelStatus.Status.IsPlaying);
    }

    public void LevelCompleted()
    {
        GoldEarned = Formula.WinningGoldEarned(UserData.Level);
        PlayerWon = true;
        GameManager.Instance.SwitchState(GameState.MainMenu);
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(LevelStatus.Status.Completed);
        UserData.Level++;
        if (UserData.Level > 1 && !UserData.StartCounting)
        {
            UserData.CurrentPlayTimeFormatted = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            UserData.StartCounting = true;
        }
        JSONSaving.Instance.UserData.Level = UserData.Level;
        SetLevel();      
        levelGenerator.LoadNextLevel(); 
        OnShowingLevelID?.Invoke(UserData.Level.ToString());
    }

    public void GameOver()
    {
        GoldEarned = Formula.LoseGoldEarned(UserData.Level);
        PlayerLose = true;
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }
}
