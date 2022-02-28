using System;
using UnityEngine;

public class State : MonoBehaviour, IInitializeVariables
{
    public SpriteRenderer spriteRenderer;
    public GameObject owner;

    private Building buildingOwner;
    private GlobalVariables.Building buildingType;

    private void Start()
    {
        InitializeVariables();
    }

    public void InitializeVariables()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingOwner = owner.GetComponent<Building>();
        buildingType = buildingOwner.BuildingType;
        Utilities.SetStateColor(buildingType, spriteRenderer);
    }
}
