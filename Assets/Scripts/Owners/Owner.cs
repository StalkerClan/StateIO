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
    public List<Building> startBuildings;
    private HashSet<Building> hashSetStartBuildings;
    public int OwnerID; 

    public OwnerStat OwnerStat { get => ownerStat; set => ownerStat = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }  
    public List<Building> StartBuildings { get => startBuildings; set => startBuildings = value; }
    public HashSet<Building> HashSetStartBuildings { get => hashSetStartBuildings; set => hashSetStartBuildings = value; }

    private void Start()
    {
        startBuildings = hashSetStartBuildings.ToList();
    }

    public void AddBuilding(Building building)
    {
        hashSetStartBuildings.Add(building);
        OnAddedBuilding?.Invoke(building);
        building.GetBuildingStats(this);
        startBuildings = hashSetStartBuildings.ToList();
    }

    public void RemoveBuilding(Building building)
    {
        hashSetStartBuildings.Remove(building);
        OnRemovedBuilding?.Invoke(building);
        startBuildings = hashSetStartBuildings.ToList();
    }

    public  void ChangeColor(ColorSet colorSet)
    {
        ownerStat.ColorSet = colorSet;
        OnChangingColorSet?.Invoke(colorSet);
    }
}
