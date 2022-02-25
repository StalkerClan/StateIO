using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticManager : MonoBehaviour
{
    public static AnalyticManager Instance { private get; set; }


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }   
        
        Instance = this;
        DontDestroyOnLoad(this);
    }


}
