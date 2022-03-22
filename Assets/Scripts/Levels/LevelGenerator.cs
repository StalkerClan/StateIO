using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public event Action OnEnemiesOutOfBuildings = delegate { };
    public event Action OnPlayerOutOfBuildings = delegate { };

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject neutralPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Level> listLevel;
    [SerializeField] private Level currentLevel;
    [SerializeField] private Owner playerData;
    [SerializeField] private Owner neutralData;
    [SerializeField] private List<Owner> enemiesInfo = new List<Owner>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private int enemiesOwnedBuildings;
    [SerializeField] private int playerOwnedBuildings;

    public List<Level> ListLevel { get => listLevel; set => listLevel = value; }
    public Level CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public Owner PlayerData { get => playerData; set => playerData = value; }
    public List<Owner> EnemiesInfo { get => enemiesInfo; set => enemiesInfo = value; }

    public void SetFocusPoint()
    {    
        CameraController.Instance.DesiredPosition = currentLevel.FocusPoint;
        CameraController.Instance.DesiredSize = currentLevel.CamZoomSize;
    }

    public void InitializePlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        this.playerData = player.GetComponent<Owner>();
        this.playerData.OwnerStat = JSONSaving.Instance.UserStat;
    }

    public void InitializeNeutral()
    {
        GameObject newNeutral = Instantiate(neutralPrefab);
        neutralData = newNeutral.GetComponent<Owner>();            
    }

    public void InitializeEnemies()
    {
        int loop = currentLevel.EnemyInfos.Count - enemies.Count <= 0 ? currentLevel.EnemyInfos.Count : currentLevel.EnemyInfos.Count - enemies.Count;
        for (int i = 0; i < loop; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            Owner enemyInfo = newEnemy.GetComponent<Owner>();         
            for (int j = 1; j < UltilitiesManager.Instance.ColorSets.Count; j++)
            {
                if (!UltilitiesManager.Instance.ColorSets[j].PlayerUsed)
                {
                    if (!UltilitiesManager.Instance.ColorSets[j].EnemyUsed)
                    {
                        enemyInfo.ColorSet = UltilitiesManager.Instance.ColorSets[j];
                        enemyInfo.ColorSet.EnemyUsed = true;
                        break;
                    }
                }                 
            }
            enemies.Add(newEnemy);
            enemiesInfo.Add(enemyInfo);
        }
    }

    public void SetBuildingDefaultOwner(List<Building> buildings, Owner owner)
    {
        owner.HashSetStartBuildings = new HashSet<Building>(buildings);
        foreach (Building building in buildings)
        {
            building.DefaultOwner = owner;
        }
        owner.StartBuildings = owner.HashSetStartBuildings.ToList();
    }

    public void SetBuildingOwner(List<Building> buildings, Owner owner)
    {
        owner.HashSetStartBuildings = new HashSet<Building>(buildings);
        foreach (Building building in buildings)
        {
            building.DefaultOwner = owner;
            building.SetOwner(owner, owner.OwnerType);
        }
        owner.StartBuildings = owner.HashSetStartBuildings.ToList();
    }

    public void SetPlayerStartBuildings()
    {
        SetBuildingDefaultOwner(currentLevel.PlayerStartBuildings, playerData);
        playerOwnedBuildings = playerData.HashSetStartBuildings.Count;
    }

    public void SetNeutralStartBuildings()
    {
        SetBuildingDefaultOwner(currentLevel.NeutralStartBuildings, neutralData);
    }

    public void SetEnemiesStartBuildings()
    {
        for (int i = 0; i < currentLevel.EnemyInfos.Count; i++)
        {
            SetBuildingDefaultOwner(currentLevel.EnemyInfos[i].EnemyStartBuildings, enemiesInfo[i]);                          
            foreach (Enemy enemy in enemiesInfo)
            {
                enemiesOwnedBuildings += enemy.StartBuildings.Count;
            }
        }           
    }   

    public void LoadCurrentLevel()
    {
        SetFocusPoint();
        SetPlayerStartBuildings();
        SetNeutralStartBuildings();
        SetEnemiesStartBuildings();
    }

    public void LoadNextLevel()
    {
        if (enemies.Count < currentLevel.EnemyInfos.Count)
        {
            InitializeEnemies();
        }
        SetFocusPoint();
        SetBuildingOwner(currentLevel.PlayerStartBuildings, playerData);
        SetBuildingOwner(currentLevel.NeutralStartBuildings, neutralData);
        for (int i = 0; i < currentLevel.EnemyInfos.Count; i++)
        {
            SetBuildingOwner(currentLevel.EnemyInfos[i].EnemyStartBuildings, enemiesInfo[i]);
        }
    }
   
    public void EnableGenerateFighter()
    {
        StartCoroutine(StartGeneratingFighter());
    }

    public void EnableGenerateFighter(List<Building> buildings, bool active, bool isGenerating)
    {
        foreach (Building building in buildings)
        {
            building.Active = active;
            building.IsGenerating = isGenerating;
        }
    }

    IEnumerator StartGeneratingFighter()
    {
        WaitForSeconds delay = Utilities.GetWaitForSeconds(1.5f);
        yield return delay;

        EnableGenerateFighter(currentLevel.PlayableBuildings, true, true);
    }

    public void SetBuildingToDefault()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.SetOwner(building.DefaultOwner, building.DefaultOwnerType);
        }       
    }

    public void SetPlayerOwnedBuildings()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.DefaultOwner = playerData;
            building.SetOwner(playerData, playerData.OwnerType);
        }
    }

    public void CheckWinCondition()
    {
        enemiesOwnedBuildings = 0;
        playerOwnedBuildings = 0;

        foreach (Enemy enemy in enemiesInfo)
        {
            enemiesOwnedBuildings += enemy.StartBuildings.Count;
        }

        playerOwnedBuildings = playerData.StartBuildings.Count;

        if (enemiesOwnedBuildings <= 0)
        {
            OnEnemiesOutOfBuildings?.Invoke();
        }

        if (playerOwnedBuildings <= 0)
        {
            OnPlayerOutOfBuildings?.Invoke();
        }
    }

    public void LoadMap()
    {
        InitializePlayer();
        InitializeNeutral();
        InitializeEnemies();

        foreach (Level level in listLevel)
        {
            LevelStatus.Status status = level.LevelStatus.CurrentStatus;
            switch (status)
            {
                case LevelStatus.Status.Completed:
                    SetBuildingDefaultOwner(level.PlayableBuildings, playerData);
                    break;
                case LevelStatus.Status.IsPlaying:
                    LoadCurrentLevel();
                    break;
                case LevelStatus.Status.Locked:
                    foreach (Building building in level.PlayableBuildings)
                    {
                        building.DefaultOwner = neutralData;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetParentAllLevels()
    {
        foreach (Level level in listLevel)
        {
            level.transform.parent = this.transform;
        }
    }
}
