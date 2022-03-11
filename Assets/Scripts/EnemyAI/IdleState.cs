using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {

    }

    public override void UpdateState(EnemyStateManager enemyStateManager, HashSet<Building> buildings)
    {
        if (PlayerInput.Instance.Marched)
        {
            enemyStateManager.SwitchState(enemyStateManager.EngageState);
        }
    }
}
