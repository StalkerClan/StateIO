using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeSystem : Singleton<UpgradeSystem>
{
    public StartUnits StartUnits;
    public ProduceSpeed ProduceSpeed;
    public UserData UserData;
    public Player Player;
    public int Level;

    private void Awake()
    {
        FetchData();
        GetUpgrades();
    }

    public void FetchData()
    {
        UserData = DataManager.Instance.UserData;
        Player = DataManager.Instance.Player;
        Level = UserData.Level;
    }

    public void GetUpgrades()
    {
        List<Upgrade> upgrades = GetComponentsInChildren<Upgrade>().ToList();
        StartUnits = upgrades.Find(x => x.Type == GlobalVariables.UpgradeType.StartUnits) as StartUnits;
        ProduceSpeed = upgrades.Find(x => x.Type == GlobalVariables.UpgradeType.ProduceSpeed) as ProduceSpeed;
    }
    public void SetUpgradeStartUnits()
    {
        SetUpgrade(StartUnits, StartUnits.UpgradeData, (int) Formula.StartUnits((int) StartUnits.UpgradeData.Value));
        Player.UpgradeStartUnits((int) StartUnits.UpgradeData.Value);
    }

    public void SetUpgradeProduceSpeed()
    {
        SetUpgrade(ProduceSpeed, ProduceSpeed.UpgradeData, Formula.ProduceSpeed(ProduceSpeed.UpgradeData.Level));
        Player.UpgradeProduceSpeed(ProduceSpeed.UpgradeData.Value);
    }

    public void SetUpgrade(Upgrade upgrade, UpgradeData upgradeData, float value)
    {
        upgradeData = upgrade.UpgradeData;
        upgradeData.Level++;
        upgradeData.Value = value;
        upgradeData.Cost = Formula.GoldRequired(upgradeData.Level);
        upgrade.ShowUpgradeData();
    }
}
