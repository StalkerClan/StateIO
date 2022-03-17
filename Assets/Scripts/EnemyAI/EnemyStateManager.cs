using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, ISubcriber
{
    public Enemy enemy;

    public IdleState IdleState = new IdleState();
    public EngageState EngageState = new EngageState();

    public EnemyBaseState currentState;

    private void Start()
    {
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

    private void Update()
    {
        currentState.UpdateState(this, enemy.HashSetStartBuildings);
    }

    public void SwitchState(EnemyBaseState enemyBaseState)
    {
        this.currentState = enemyBaseState;
        currentState.EnterState(this);
    }

    private void AddBuilding(Building newBuilding)
    {
        enemy.StartBuildings.Add(newBuilding);
    }
    
    private void RemoveBuilding(Building newBuilding)
    {
        enemy.StartBuildings.Remove(newBuilding);
    }
}
