using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int numberOfBuildings = 7;
    [SerializeField] private int playerBuilding = 1;
    [SerializeField] private List<Building> activeBuildings;

    public int NumberOfBuildings { get => numberOfBuildings; set => numberOfBuildings = value; }
    public int PlayerBuilding { get => playerBuilding; set => playerBuilding = value; }
    public List<Building> ActiveBuildings { get => activeBuildings; set => activeBuildings = value; }

    private void Start()
    {
        
    }

    public void CheckWinCondition()
    {
        if (playerBuilding >= numberOfBuildings)
        {
            GameManager.Instance.SwitchState(GameState.FinishedLevel);
        } 
        else if (playerBuilding <= 0)
        {
            GameManager.Instance.SwitchState(GameState.GameOver);
        }
    }


    public void EnableGeneratingFighter()
    {
        StartCoroutine(StartGeneratingFighter());
    }

    IEnumerator StartGeneratingFighter()
    {
        WaitForSeconds delay = Utilities.GetWaitForSeconds(1.5f);
        yield return delay;

        foreach (Building building in activeBuildings)
        {
            building.IsGenerating = true;
        }
    }

    public void SetBuildingToDefault()
    {
        foreach (Building building in activeBuildings)
        {
            building.SetBuildingToDefault(building.DefaultOwner, building.DefaultOwnerType);
        }
    }
}
