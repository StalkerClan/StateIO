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
    }

    public void UnsubcribeEvent()
    {
        building.OnChangingNumberOfFighters -= ShowNumberOfFighter;
    }

    private void ShowNumberOfFighter(int fighter)
    {
        textMesh.text = building.CurrentFighter.ToString();
    }
}
