using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : Singleton<UpgradeSystem>
{
    public StartUnits StartUnits;
    public ProduceSpeed ProduceSpeed;

    private void Awake()
    {
        InitUpgrades();
    }

    public void InitUpgrades()
    {
        List<Upgrade> upgrades = GetComponentsInChildren<Upgrade>().ToList();
        StartUnits = upgrades.Find(x => x.Type == GlobalVariables.UpgradeType.StartUnits) as StartUnits;
        ProduceSpeed = upgrades.Find(x => x.Type == GlobalVariables.UpgradeType.ProduceSpeed) as ProduceSpeed;
    }

    public void SetUpgradeProduceSpeed()
    {

    }

    public void SetUpgradeStartUnits()
    {

    }    
}
