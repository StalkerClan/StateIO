using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public abstract class Owner : MonoBehaviour
{
    public event Action<Building> OnAddedBuilding = delegate { };
    public event Action<Building> OnRemovedBuilding = delegate { };

    public GameObject fighterPrefab;
    public OwnerStat ownerStat;
    public GlobalVariables.Owner ownerType;
    public List<Building> ownedBuildings;

   private HashSet<Building> hashSetOwnedBuildings;

    public GameObject FighterPrefab { get => fighterPrefab; set => fighterPrefab = value; }
    public OwnerStat OwnerStat { get => ownerStat; set => ownerStat = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }
    public List<Building> OwnedBuildings { get => ownedBuildings; set => ownedBuildings = value; }

    public void AddBuilding(Building building)
    {
        hashSetOwnedBuildings.Add(building);
        OnAddedBuilding?.Invoke(building);
        building.GetBuildingStats(this);
    }

    public void RemoveBuilding(Building building)
    {
        hashSetOwnedBuildings.Remove(building);
        OnRemovedBuilding?.Invoke(building);
    }

    public void SetBuildingOwner()
    {
        hashSetOwnedBuildings = new HashSet<Building>(ownedBuildings);

        foreach (Building building in ownedBuildings)
        {
            building.GetBuildingStats(this);
        }
    }
}
