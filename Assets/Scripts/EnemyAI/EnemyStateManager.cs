using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public Enemy enemy;
    public List<Building> ownedBuildings;

    public IdleState IdleState = new IdleState();

    public EnemyBaseState currentState;

    void Start()
    {      
        currentState = IdleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, ownedBuildings);
    }

    public void SwitchState(EnemyBaseState enemyBaseState)
    {
        this.currentState = enemyBaseState;
        currentState.EnterState(this);
    }
}
