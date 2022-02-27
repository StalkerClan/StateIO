using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IInitializeVariables
{
    public GameObject fighterPrefab;
    public SpriteRenderer spriteRenderer;
    public Color buildingColor;
    public List<Vector3[]> listOfPositions;
    public List<Vector3> directions;
    public Vector3 direction;
    public Vector3 leftDirection;
    public Vector3 rightDirection;
    public Vector3 mostLeftDirection;
    public Vector3 mostRightDirection;
    public GlobalVariables.Building building;
    public FighterDirection fighterDirection;
    public List<GameObject> fighters;

    public float generatingCooldown;
    public float timeToGenerate;
    public float timer;
    public float totalTime;
    public float spacing;

    public int startFighter;
    public int maxCapacity;
    public int currentFighter;

    public bool isGenerating;
    public bool isMarching;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public FighterDirection FighterDirection { get => fighterDirection; set => fighterDirection = value; }

    public void InitializeVariables()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingColor = spriteRenderer.color;
        listOfPositions = new List<Vector3[]>();
        directions = new List<Vector3>();
        fighters = new List<GameObject>();
        spacing = 0.5f;
        isGenerating = true;
        isMarching = false;
    }

    public void GetBuildingType()
    {
        switch (building)
        {
            case GlobalVariables.Building.Player:
                startFighter = 10;
                maxCapacity = 50;
                break;
            case GlobalVariables.Building.Neutral:
                startFighter = 0;
                maxCapacity = 50;
                break;
            case GlobalVariables.Building.Opponent:
                startFighter = 10;
                maxCapacity = 50;
                break;
            default:
                break;
        }

        currentFighter = startFighter;
    }

    public void GenerateFighter()
    {
        if (!isGenerating)
        {
            return;
        }
        LimitGeneratedFighter();
        timer += Time.deltaTime;
        if (timer >= timeToGenerate)
        {
            currentFighter++;
            timeToGenerate += generatingCooldown;
            Debug.Log(currentFighter);
        }
    }

    public void LimitGeneratedFighter()
    {
        if (currentFighter >= maxCapacity)
        {
            isGenerating = false;
        }
    }
}
