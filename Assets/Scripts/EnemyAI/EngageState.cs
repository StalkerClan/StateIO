using System.Collections.Generic;
using UnityEngine;

public class EngageState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        
    }

    public override void UpdateState(EnemyStateManager enemyStateManager, HashSet<Building> buildings)
    {
        foreach (Building building in buildings)
        {
            foreach (Building nearbyBuilding in building.NearbyBuildings)
            {
                if (building.CurrentFighter > 20)
                {
                    if (nearbyBuilding.OwnerType.Equals(GlobalVariables.Owner.Neutral) ||
                        nearbyBuilding.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        building.FighterMarching(nearbyBuilding, nearbyBuilding.transform.position);
                    }
                }
            }
        }
    }
}
