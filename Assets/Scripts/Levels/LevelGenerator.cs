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
    [SerializeField] private Owner player;
    [SerializeField] private Owner neutral;
    [SerializeField] private List<Owner> enemiesInfo = new List<Owner>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private int enemiesOwnedBuildings;
    [SerializeField] private int playerOwnedBuildings;

    public List<Level> ListLevel { get => listLevel; set => listLevel = value; }
    public Level CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public Owner Player { get => player; set => player = value; }
    public List<Owner> Enemies { get => enemiesInfo; set => enemiesInfo = value; }

        
    public void SetFocusPoint()
    {    
        CameraController.Instance.DesiredPosition = currentLevel.FocusPoint;
        CameraController.Instance.DesiredSize = currentLevel.CamZoomSize;
    }

    public void InitializePlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        player = newPlayer.GetComponent<Owner>();
    }

    public void InitializeNeutral()
    {
        GameObject newNeutral = Instantiate(neutralPrefab);
        neutral = newNeutral.GetComponent<Owner>();            
    }

    public void InitializeEnemies()
    {
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            enemiesInfo.Clear();
        }

        for (int i = 0; i < currentLevel.EnemyInfos.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            Owner enemyInfo = newEnemy.GetComponent<Owner>();
            enemyInfo.OwnerStat = UltilitiesManager.Instance.EnemiesStats[i];
            enemies.Add(newEnemy);
            enemiesInfo.Add(enemyInfo);
        }
    }

    public void SetPlayerStartBuildings()
    {
        player.HashSetOwnedBuildings = new HashSet<Building>(currentLevel.PlayerStartBuildings);
        player.OwnedBuildings = player.HashSetOwnedBuildings.ToList();
        foreach (Building building in currentLevel.PlayerStartBuildings)
        {
            building.DefaultOwner = player;
            building.SetOwner(player, player.OwnerType);
        }
    }

    public void SetNeutralStartBuildings()
    {
        neutral.HashSetOwnedBuildings = new HashSet<Building>(currentLevel.NeutralStartBuildings);
        neutral.OwnedBuildings = neutral.HashSetOwnedBuildings.ToList();
        foreach (Building building in currentLevel.NeutralStartBuildings)
        {
            building.DefaultOwner = neutral;
            building.SetOwner(neutral, neutral.OwnerType);
        }
    }

    public void SetEnemiesStartBuildings()
    {
        for (int i = 0; i < currentLevel.EnemyInfos.Count; i++)
        {
            enemiesInfo[i].HashSetOwnedBuildings = new HashSet<Building>(currentLevel.EnemyInfos[i].EnemyStartBuildings);
            enemiesInfo[i].OwnedBuildings = enemiesInfo[i].HashSetOwnedBuildings.ToList();
            foreach (Building building in currentLevel.EnemyInfos[i].EnemyStartBuildings)
            {
                building.DefaultOwner = enemiesInfo[i];
                building.SetOwner(enemiesInfo[i], enemiesInfo[i].OwnerType);
            }
        }           
    }

    public void LoadLevel()
    {       
        InitializeEnemies();
        SetFocusPoint();
        SetPlayerStartBuildings();
        SetNeutralStartBuildings();
        SetEnemiesStartBuildings();
    }
    public void ChangePlayerColorToBlue()
    {
        player.ChangeColor(UltilitiesManager.Instance.PlayerColorSets[0]);
        enemiesInfo[0].ChangeColor(UltilitiesManager.Instance.EnemiesColorSets[0]);
    }

    public void ChangePlayerColorToRed()
    {
        player.ChangeColor(UltilitiesManager.Instance.EnemiesColorSets[0]);
        enemiesInfo[0].ChangeColor(UltilitiesManager.Instance.PlayerColorSets[0]);
    }
   
    public void EnableGenerateFighter()
    {
        StartCoroutine(StartGeneratingFighter());
    }

    public void DisableGenerateFighter()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.Active = false;
            building.IsGenerating = false;
        }
    }

    IEnumerator StartGeneratingFighter()
    {
        WaitForSeconds delay = Utilities.GetWaitForSeconds(1.5f);
        yield return delay;

        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.Active = true;
            building.IsGenerating = true;
        }
    }

    public void SetBuildingToDefault()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.SetOwner(building.DefaultOwner, building.DefaultOwnerType);
            if (building.CurrentFighter >= building.StartFighter) building.CurrentFighter = building.StartFighter;
        }
    }
    public void SetPlayerOwnedBuildings()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.DefaultOwner = player;
            building.SetOwner(player, player.OwnerType);
            if (building.CurrentFighter >= building.StartFighter) building.CurrentFighter = building.StartFighter;
        }
    }

    public void CheckWinCondition()
    {
        enemiesOwnedBuildings = 0;
        playerOwnedBuildings = 0;

        foreach (Enemy enemy in enemiesInfo)
        {
            enemiesOwnedBuildings += enemy.OwnedBuildings.Count;
        }

        playerOwnedBuildings = player.OwnedBuildings.Count;
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
            if (level.Status.Completed)
            {
                foreach (Building building in level.PlayableBuildings)
                {
                    building.DefaultOwner = player;
                    building.SetOwner(player, player.OwnerType);
                }
            }
            if (level.Status.IsPlaying)
            {
                LoadLevel();
            }
            if (level.Status.Locked)
            {
                foreach (Building building in level.PlayableBuildings)
                {
                    building.DefaultOwner = neutral;
                    building.SetOwner(neutral, neutral.OwnerType);
                }
            }
        }
    }
}
