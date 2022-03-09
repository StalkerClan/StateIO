using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : Owner
{
    private void OnEnable()
    {
        SetBuildingOwner();
    }
}
