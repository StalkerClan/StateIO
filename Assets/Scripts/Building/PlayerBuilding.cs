using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBuilding : Building, ISubcriber
{
    private void Awake()
    {
        owned = true;
        InitializeVariables();
        GetBuildingType();
        SubcribeEvent();
    }
    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    public void SubcribeEvent()
    {
        playerInput.OnMarching += FighterMarching;
    }
    public void UnsubcribeEvent()
    {
        playerInput.OnMarching -= FighterMarching;
    }

    private void Update()
    {
        GenerateFighter();

        if (!isMarching)
        {
            CancelInvoke("InitializeFighter");
        }
    }

}
