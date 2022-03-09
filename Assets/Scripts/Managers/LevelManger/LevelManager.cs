using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int numberOfBuildings = 7;
    [SerializeField] private int playerBuilding = 1;

    public int NumberOfBuildings { get => numberOfBuildings; set => numberOfBuildings = value; }
    public int PlayerBuilding { get => playerBuilding; set => playerBuilding = value; }

    private void Start()
    {
        
    }

    public void CheckPlayerOwnedBuilding()
    {
        if (playerBuilding >= numberOfBuildings)
        {
            GameManager.Instance.SwitchState(GameManager.Instance.FinishedLevelState);
        } 
        else if (playerBuilding <= 0)
        {
            Debug.Log("haha");
            GameManager.Instance.SwitchState(GameManager.Instance.GameOverState);
        }
    }
}
