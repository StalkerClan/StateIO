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

    public string stateName;

    public float currentFighter;
    public float buildingMaxCapacity;
    
    private void Start()
    {
        InitializeVariables();
        SetOwner(stateOwner);
        SetStateAndBuildingName();
        SubcribeEvent();
    }

    private void Update()
    {
        ChangeColorOverTime();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }
    public void SubcribeEvent()
    {      
        building.OnChangingOnwer += SetOwner;
    }

    public void UnsubcribeEvent()
    {
        building.OnChangingOnwer -= SetOwner; 
        stateOwner.OnChangingColorSet -= ChangeStateColor;
    }

    public void InitializeVariables()
    {
        stateOwner = building.BuildingOwner;
    }

    private void SetStateAndBuildingName()
    {
        stateName = spriteRenderer.sprite.name;
        stateName = stateName.Remove(0, 3);
        stateName = char.ToUpper(stateName[0]) + stateName.Substring(1);
        this.gameObject.name = stateName + "State";
        building.gameObject.name = building.BuildingID = stateName + "Building";
    }

    public void SetOwner(Owner owner)
    {
        if (stateOwner != null)
        {
            stateOwner.OnChangingColorSet -= ChangeStateColor;
        }
        stateOwner = owner;
        stateOwner.OnChangingColorSet += ChangeStateColor;
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(stateOwner.OwnerStat.ColorSet.firstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(stateOwner.OwnerStat.ColorSet.secondColor));
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    public void ChangeStateColor(ColorSet newColorSet)
    {
       
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.firstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.secondColor));
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    public void ChangeColorOverTime()
    {              
        currentFighter = building.CurrentFighter;
        buildingMaxCapacity = building.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }
}
