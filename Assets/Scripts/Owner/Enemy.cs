using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Owner
{
    private void OnEnable()
    {
        SetBuildingOwner();
    }
}
