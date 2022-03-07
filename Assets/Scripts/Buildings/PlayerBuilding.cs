using System;
using UnityEngine;

public class PlayerBuilding : Building
{
    public event Action IsFirstTimeMarching = delegate { };

    private void Start()
    {
        InitializeVariables();
        GetBuildingStats(this);
        GetBuildingType();
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    public override void SubcribeEvent()
    {
        PlayerInput.Instance.OnSelectedTarget += FighterMarching;
        OnChangingOnwer += GetBuildingStats;
        OnChangingNumberOfFighters += ChangeNumberOfFighter;
    }
    public override void UnsubcribeEvent()
    {
        PlayerInput.Instance.OnSelectedTarget -= FighterMarching;
        OnChangingOnwer -= GetBuildingStats;
        OnChangingNumberOfFighters -= ChangeNumberOfFighter;
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