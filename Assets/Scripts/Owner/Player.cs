using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Owner
{
    private void OnEnable()
    {
        SetBuildingOwner();
 
    }

    private void OnDisable()
    {

    }
}
