using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public abstract class Owner : MonoBehaviour
{
    public event Action<Building> OnAddedBuilding = delegate { };
    public event Action<Building> OnRemovedBuilding = delegate { };
    public event Action<ColorSet> OnChangingColorSet = delegate { };

    public OwnerStat ownerStat;
    public GlobalVariables.Owner ownerType;
    public List<Building> ownedBuildings;
    private HashSet<Building> hashSetOwnedBuildings;
    public int OwnerID; 

    public OwnerStat OwnerStat { get => ownerStat; set => ownerStat = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }  
    public List<Building> OwnedBuildings { get => ownedBuildings; set => ownedBuildings = value; }
    public HashSet<Building> HashSetOwnedBuildings { get => hashSetOwnedBuildings; set => hashSetOwnedBuildings = value; }

    private void Start()
    {
        ownedBuildings = hashSetOwnedBuildings.ToList();
    }

    public void AddBuilding(Building building)
    {
        hashSetOwnedBuildings.Add(building);
        OnAddedBuilding?.Invoke(building);
        building.GetBuildingStats(this);
        ownedBuildings = hashSetOwnedBuildings.ToList();
    }

    public void RemoveBuilding(Building building)
    {
        hashSetOwnedBuildings.Remove(building);
        OnRemovedBuilding?.Invoke(building);
        ownedBuildings = hashSetOwnedBuildings.ToList();
    }

    public  void ChangeColor(ColorSet colorSet)
    {
        ownerStat.colorSet = colorSet;
        OnChangingColorSet?.Invoke(colorSet);
    }
}
