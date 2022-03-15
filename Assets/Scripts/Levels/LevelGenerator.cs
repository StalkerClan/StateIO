using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject neutralPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Level> listLevel;
    [SerializeField] private Level currentLevel;
    [SerializeField] private Owner player;
    [SerializeField] private Owner neutral;
    [SerializeField] private List<Owner> enemies = new List<Owner>();

    public List<Level> ListLevel { get => listLevel; set => listLevel = value; }
    public Level CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public Owner Player { get => player; set => player = value; }
    public List<Owner> Enemies { get => enemies; set => enemies = value; }

        
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
        for (int i = 0; i < currentLevel.EnemyInfos.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            Owner enemy = newEnemy.GetComponent<Owner>();
            enemies.Add(enemy);
        }
    }

    public void SetPlayerStartBuildings()
    {
        player.HashSetOwnedBuildings = new HashSet<Building>(currentLevel.PlayerStartBuildings);
        foreach (Building building in currentLevel.PlayerStartBuildings)
        {
            building.DefaultOwner = player;
            building.SetOwner(player, player.OwnerType);
        }
    }

    public void SetNeutralStartBuildings()
    {
        neutral.HashSetOwnedBuildings = new HashSet<Building>(currentLevel.NeutralStartBuildings);
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
            enemies[i].HashSetOwnedBuildings = new HashSet<Building>(currentLevel.EnemyInfos[i].EnemyStartBuildings);
            foreach (Building building in currentLevel.EnemyInfos[i].EnemyStartBuildings)
            {
                building.DefaultOwner = enemies[i];
                building.SetOwner(enemies[i], enemies[i].OwnerType);
            }
        }           
    }

    public void LoadLevel()
    {
        SetFocusPoint();
        SetPlayerStartBuildings();
        SetNeutralStartBuildings();
        SetEnemiesStartBuildings();
    }

    public void ChangePlayerColorToRed()
    {
        player.ChangeColor(CosmeticManager.Instance.ColorSets[0]);
        enemies[0].ChangeColor(CosmeticManager.Instance.ColorSets[1]);
    }

    public void ChangePlayerColorToBlue()
    {
        player.ChangeColor(CosmeticManager.Instance.ColorSets[1]);
        enemies[0].ChangeColor(CosmeticManager.Instance.ColorSets[0]);
    }
   
    public void EnableGenerateFighter()
    {
        StartCoroutine(StartGeneratingFighter());
    }

    public void DisableGenerateFighter()
    {
        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.Idle = true;
            building.IsGenerating = false;
        }
    }

    IEnumerator StartGeneratingFighter()
    {
        WaitForSeconds delay = Utilities.GetWaitForSeconds(1.5f);
        yield return delay;

        foreach (Building building in currentLevel.PlayableBuildings)
        {
            building.Idle = false;
            building.IsGenerating = true;
        }
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
            building.DefaultOwner = player;
            building.SetOwner(player, player.OwnerType);
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
