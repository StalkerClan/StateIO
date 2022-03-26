using System;
using UnityEngine;

public class ShowNumber : MonoBehaviour, ISubcriber
{
    public Building building;
    public TextMesh textMesh;

    private void Start()
    {
        ShowNumberOfFighter(building.CurrentFighter);
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    public void SubcribeEvent()
    {
        building.OnChangingNumberOfFighters += ShowNumberOfFighter;
        building.OnEnableBuilding += ShowNumberOfFighter;
        building.OnDisableBuilding += DisableNumber;
    }

    public void UnsubcribeEvent()
    {
        building.OnChangingNumberOfFighters -= ShowNumberOfFighter;
        building.OnEnableBuilding -= ShowNumberOfFighter;
        building.OnDisableBuilding -= DisableNumber;
    }

    private void ShowNumberOfFighter(float fighter)
    {
        textMesh.text = ((int) fighter).ToString();
    }

    private void DisableNumber()
    {
        textMesh.text = "";
    }
}
