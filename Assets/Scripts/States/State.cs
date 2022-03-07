using System;
using UnityEngine;

public class State : MonoBehaviour, IInitializeVariables, ISubcriber
{
    public SpriteRenderer spriteRenderer;
    public Building owner;

    public Color currentColor;
    public Color firstColor;
    public Color secondColor;

    public string stateName;

    public float currentFighter;
    public float buildingMaxCapacity;
    
    private void Start()
    {
        InitializeVariables();
        SetOwner(owner);
        SetStateName(owner);
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }
    public void SubcribeEvent()
    {
        owner.OnChangingOnwer += SetOwner;
    }

    public void UnsubcribeEvent()
    {
        owner.OnChangingOnwer -= SetOwner;
    }

    public void InitializeVariables()
    {
        owner = owner.GetComponent<Building>();
    }

    private void SetStateName(Building owner)
    {
        stateName = spriteRenderer.sprite.name;
        stateName = stateName.Remove(0, 3);
        stateName = char.ToUpper(stateName[0]) + stateName.Substring(1);
        this.gameObject.name = stateName + "State";
        owner.gameObject.name = owner.BuildingID = stateName + "Building";
    }

    public void SetOwner(Building owner)
    {
        firstColor = owner.FirstColor;
        secondColor = owner.SecondColor;
        buildingMaxCapacity = owner.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    public void ChangeColorOverTime()
    {              
        currentFighter = owner.CurrentFighter;
        buildingMaxCapacity = owner.MaxCapacity;
        float percent = currentFighter / buildingMaxCapacity;
        currentColor = Color.Lerp(firstColor, secondColor, percent);
        spriteRenderer.color = currentColor;
    }

    private void Update()
    {
        ChangeColorOverTime();        
    }
}
