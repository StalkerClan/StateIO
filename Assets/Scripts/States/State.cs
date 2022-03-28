using System;
using UnityEngine;

public class State : MonoBehaviour, IInitializeVariables, ISubcriber
{
    public SpriteRenderer spriteRenderer;
    public Building building;
    public Owner stateOwner;

    public Color currentColor;
    public Color firstColor;
    public Color secondColor;

    public float currentFighter;
    public float buildingMaxCapacity;
    
    private void Start()
    {
        InitializeVariables();
        SetOwner(stateOwner);
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }
    public void SubcribeEvent()
    {      
        building.OnChangingOnwer += SetOwner;
        building.OnEnableBuilding += EnableState;
        building.OnDisableBuilding += DisableState;
        building.OnChangingNumberOfFighters += ChangeColorOverTime;
    }

    public void UnsubcribeEvent()
    {
        building.OnChangingOnwer -= SetOwner;
        building.OnEnableBuilding -= EnableState;
        building.OnDisableBuilding -= DisableState;
        stateOwner.OnChangingColorSet -= ChangeStateColor;
        building.OnChangingNumberOfFighters -= ChangeColorOverTime;
    }

    public void InitializeVariables()
    {
        stateOwner = building.BuildingOwner;
    }

    public void DisableState()
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = 0.2f;
        spriteRenderer.color = tempColor;
    } 
    
    public void EnableState(float value)
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = 1f;
        spriteRenderer.color = tempColor;
    }
    
    public void SetOwner(Owner owner)
    {
        if (stateOwner != null)
        {
            stateOwner.OnChangingColorSet -= ChangeStateColor;
        }
        stateOwner = owner;
        stateOwner.OnChangingColorSet += ChangeStateColor;
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(stateOwner.ColorSet.FirstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(stateOwner.ColorSet.SecondColor));
        currentFighter = building.CurrentFighter;
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    public void ChangeStateColor(ColorSet newColorSet)
    {   
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.FirstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.SecondColor));
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    public void ChangeColorOverTime(float fighter)
    {
        currentFighter = building.CurrentFighter;
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }
}
