using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, ISubcriber
{
    public Enemy enemy;
    public HashSet<Building> ownedBuildings;

    public IdleState IdleState = new IdleState();

    public EnemyBaseState currentState;

    private void Start()
    {
        ownedBuildings =  new HashSet<Building>(enemy.OwnedBuildings);
        currentState = IdleState;
        currentState.EnterState(this);
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    public void SubcribeEvent()
    {
        enemy.OnAddedBuilding += AddBuilding;
        enemy.OnRemovedBuilding += RemoveBuilding;
    }

    public void UnsubcribeEvent()
    {
        enemy.OnAddedBuilding -= AddBuilding;
        enemy.OnRemovedBuilding -= RemoveBuilding;
    }


    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState(this, ownedBuildings);
    }

    public void SwitchState(EnemyBaseState enemyBaseState)
    {
        this.currentState = enemyBaseState;
        currentState.EnterState(this);
    }

    private void AddBuilding(Building newBuilding)
    {
        ownedBuildings.Add(newBuilding);
    }
    
    private void RemoveBuilding(Building newBuilding)
    {
        ownedBuildings.Remove(newBuilding);
    }
}
