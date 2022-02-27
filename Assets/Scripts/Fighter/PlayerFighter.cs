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

    }

    private void Update()
    {
        Marching();
        //Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OpponentBuilding") || collision.CompareTag("NeutralBuilding"))
        {

        }
    }
}
