using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
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

    public void SetLevelStatus(bool completed, bool isPlaying, bool locked)
    {
        Status.Completed = completed;
        Status.IsPlaying = isPlaying;
        Status.Locked = locked;
    }
}
