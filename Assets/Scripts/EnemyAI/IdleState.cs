using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {

    }

    public override void UpdateState(EnemyStateManager enemyStateManager, List<Building> buildings)
    {
        foreach (Building building in buildings)
        {
            foreach (Building nearbyBuilding in building.NearbyBuildings)
            {
                if (building.CurrentFighter > 12)
                {
                    if (nearbyBuilding.OwnerType.Equals(GlobalVariables.Owner.Neutral) ||
                        nearbyBuilding.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        building.FighterMarching(nearbyBuilding.BuildingID, nearbyBuilding.transform.position);
                    }
                }
            }
        }
    }
}
