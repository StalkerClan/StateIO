using System;
using UnityEngine;

public class NeutralBuilding : Building, ISubcriber
{

    private void Awake()
    {
        GetBuildingType();
        InitializeVariables();
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
        CheckBuildingType();
        if (!isMarching)
        {
            CancelInvoke("InitializeFighter");
        }
    }

    private void CheckBuildingType()
    {
        if (buildingType == GlobalVariables.Building.Player ||
            buildingType == GlobalVariables.Building.Opponent)
        {
            GenerateFighter();
        }
    }

}
