using System;
using UnityEngine;

public class UpgradeSystem : Singleton<UpgradeSystem>
{
    public event Action<string> OnBuyingUpgrade = delegate { };
    public event Action<int> OnShowingGold = delegate { };

    public Upgrade StartUnits;
    public Upgrade ProduceSpeed;
    public Upgrade OfflineEarnings;
    public UserData UserData;
    public Player Player;

    private void Awake()
    {
        FetchData();
    }

    public void FetchData()
    {
        UserData = JSONSaving.Instance.UserData;
        Player = LevelManager.Instance.LevelGenerator.PlayerData as Player;
    }

    public int AddGold(int gold)
    {
        UserData.UserStat.Gold += gold;
        return UserData.UserStat.Gold;
    }

    public void UpgradeStartUnits()
    {
        SetUpgrade(StartUnits, StartUnits.UpgradeData, Formula.StartUnits((int) StartUnits.UpgradeData.Value));
        Player.UpgradeStartUnits((int) StartUnits.UpgradeData.Value);
        
    }

    public void UpgradeProduceSpeed()
    {
        SetUpgrade(ProduceSpeed, ProduceSpeed.UpgradeData, Formula.ProduceSpeed(ProduceSpeed.UpgradeData.Level));
        Player.UpgradeProduceSpeed(ProduceSpeed.UpgradeData.Value);
    }

    public void UpgradeGoldEarningOffline()
    {
        SetUpgrade(OfflineEarnings, OfflineEarnings.UpgradeData, (int) Formula.OfflineEarning(OfflineEarnings.UpgradeData.Level));
        Player.UpgradeOfflineEarning((int) OfflineEarnings.UpgradeData.Value);
    }

    public void SetUpgrade(Upgrade upgrade, UpgradeData upgradeData, float value)
    {
        UIMainMenu mainMenuUI = UIManager.Instance.CurrentCanvas as UIMainMenu;
        upgradeData = upgrade.UpgradeData;
        if (upgradeData.Cost < Player.OwnerStat.Gold) 
        {
            Player.OwnerStat.Gold -= upgradeData.Cost;
            upgradeData.Level++;
            upgradeData.Value = value;
            upgradeData.Cost = Formula.UpgradeCost(upgradeData.Level);
            upgrade.ShowUpgradeData();
            OnBuyingUpgrade?.Invoke(Player.OwnerStat.Gold.ToString());
            mainMenuUI.UpdateTextInfo(mainMenuUI.TxtGoldMainMenu, Player.OwnerStat.Gold.ToString());
        }      
    }

    public int GoldOfflineEarnings()
    {
        int baseGoldEarningOffline = (int) OfflineEarnings.UpgradeData.Value;
        return Formula.GoldOfflineEarned(JSONSaving.Instance.LastTimePlayed, baseGoldEarningOffline);
    }

    public string TimeAway()
    {
        return Formula.TimeAway(JSONSaving.Instance.LastTimePlayed);
    }    
}
