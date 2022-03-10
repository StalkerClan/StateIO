using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState 
{
    public abstract void EnterState(EnemyStateManager enemyStateManager);

    public abstract void UpdateState(EnemyStateManager enemyStateManager, HashSet<Building> buildings);

}
