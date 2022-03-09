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
        SetStateName(building);
        SubcribeEvent();
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
    }

    public void InitializeVariables()
    {
        building = GetComponentInChildren<Building>();
        stateOwner = building.BuildingOwner;
    }

    private void SetStateName(Building building)
    {
        stateName = spriteRenderer.sprite.name;
        stateName = stateName.Remove(0, 3);
        stateName = char.ToUpper(stateName[0]) + stateName.Substring(1);
        this.gameObject.name = stateName + "State";
        building.gameObject.name = building.BuildingID = stateName + "Building";
    }

    public void SetOwner(Owner owner)
    {
        stateOwner = owner;
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(owner.OwnerStat.colorSet.firstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(owner.OwnerStat.colorSet.secondColor));
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

    private void Update()
    {
        ChangeColorOverTime();        
    }
}
