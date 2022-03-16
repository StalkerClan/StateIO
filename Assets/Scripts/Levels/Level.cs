using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInfo
    {
        public List<Building> EnemyStartBuildings;
    }

    public GameObject FocusPoint;
    public float CamZoomSize;
    public List<Building> PlayerStartBuildings;
    public List<EnemyInfo> EnemyInfos;    
    public List<Building> NeutralStartBuildings;
    public List<Building> PlayableBuildings;
    public LevelStatus Status;

    private void OnEnable()
    {
        PlayableBuildings.AddRange(PlayerStartBuildings);
        foreach (EnemyInfo enemyInfo in EnemyInfos)
        {
            PlayableBuildings.AddRange(enemyInfo.EnemyStartBuildings);
        }
        PlayableBuildings.AddRange(NeutralStartBuildings);
    }

    public void SetLevelStatus(bool completed, bool isPlaying, bool locked)
    {
        Status.Completed = completed;
        Status.IsPlaying = isPlaying;
        Status.Locked = locked;
    }
}
