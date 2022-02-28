using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : Fighter
{
    private Coroutine MoveIE;

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
        CheckCurrentPosition();
        //Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalVariables.OPPONENT_BUILDING_TAG) || 
            collision.CompareTag(GlobalVariables.NEUTRAL_BUILDING_TAG))
        {
            if (collision.TryGetComponent(out Building building))
            {
                if (targetID.Equals(building.BuildingID))
                {
                    IReceiver receiver = collision.GetComponent<IReceiver>();
                    if (receiver != null)
                    {
                        receiver.ReceiveFighter(1, this.buildingOwner);
                        Destroy(this.gameObject);
                    }
                }
            }    
        }
    }
}
