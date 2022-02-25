using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : Fighter
{
    Coroutine MoveIE;

    private void Start()
    {
        GetFighter();
        InitializeVariables();
        GetPositions();
    }

    private void GetPositions()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log(positions[i]);
        }
    }

    private void Update()
    {
        Marching();
    }
}
