using TMPro;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    public GameObject PMainMenu;
    public GameObject POfflineEarnings;
    public GameObject PColorSwitcher;
    public GameObject PUpgrades;
    public GameObject PWon;
    public GameObject PLose;
    public TextMeshProUGUI TxtTimeAway;
    public TextMeshProUGUI TxtGoldOfflineEarnings;
    public TextMeshProUGUI TxtGoldWinning;
    public TextMeshProUGUI TxtGoldLosing;
    public TextMeshProUGUI TxtGoldMainMenu;
    public TextMeshProUGUI TxtNoThanks;
    public InversePendulum InversePendulum;
    public int GoldEarned;

    public bool showOfflineEarnings;

    public void OpenPanel()
    {
        int playerGold = JSONSaving.Instance.UserData.UserStat.Gold;
        UpdateTextInfo(TxtGoldMainMenu, playerGold.ToString());
        if (LevelManager.Instance.PlayerWon)
        {
            LevelManager.Instance.PlayerWon = false;
            OnWinning(LevelManager.Instance.GoldEarned);
            return;
        }
        
        if (LevelManager.Instance.PlayerLose)
        {
            LevelManager.Instance.PlayerWon = false;
            OnLosing(LevelManager.Instance.GoldEarned);
            return;
        }

        if (JSONSaving.Instance.UserData.Level <= 1)
        {
            POfflineEarnings.SetActive(false);
            PColorSwitcher.SetActive(true);
            PUpgrades.SetActive(false);
        }
        else
        {
            if (!JSONSaving.Instance.UserData.PassedFirstLevel)
            {
                POfflineEarnings.SetActive(false);
                PColorSwitcher.SetActive(false);
                PUpgrades.SetActive(true);
            }
            else
            {
                if (!showOfflineEarnings)
                {
                    PUpgrades.SetActive(true);
                    POfflineEarnings.SetActive(true);
                    PColorSwitcher.SetActive(false);
                    UpdateTextInfo(TxtTimeAway, UpgradeSystem.Instance.TimeAway());
                    UpdateTextInfo(TxtGoldOfflineEarnings, UpgradeSystem.Instance.GoldOfflineEarnings().ToString());
                    showOfflineEarnings = true;
                }
                else
                {
                    POfflineEarnings.SetActive(false);
                    PColorSwitcher.SetActive(false);
                    PUpgrades.SetActive(true);
                }
            }
        }

        //{
        //    PColorSwitcher.SetActive(false);
        //    PUpgrades.SetActive(true);            
        //}
        //else
        //{
        //    PColorSwitcher.SetActive(true);
        //    PUpgrades.SetActive(false);
        //}

        //if (!JSONSaving.Instance.UserData.PassedFirstLevel)
        //{
        //    POfflineEarnings.SetActive(false);
        //}
        //else
        //{
        //    if (!showOfflineEarnings)
        //    {
        //        PUpgrades.SetActive(true);
        //        POfflineEarnings.SetActive(true); 
        //        PColorSwitcher.SetActive(false);
        //        UpdateTextInfo(TxtTimeAway, UpgradeSystem.Instance.TimeAway());
        //        UpdateTextInfo(TxtGoldOfflineEarnings, UpgradeSystem.Instance.GoldOfflineEarnings().ToString());
        //        showOfflineEarnings = true;
        //    }
        //    else
        //    {
        //        POfflineEarnings.SetActive(false);
        //        PColorSwitcher.SetActive(false);
        //        PUpgrades.SetActive(true);
        //    }
        //}
    }

    public void TapToStart()
    {
        GameManager.Instance.SwitchState(GameState.GameStart);
    }

    public void ChangePlayerColorToBlue()
    {
        LevelManager.Instance.LevelGenerator.PlayerData.ChangeColor(CosmeticManager.Instance.ColorSets[1]);
        LevelManager.Instance.LevelGenerator.EnemiesInfo[0].ChangeColor(CosmeticManager.Instance.ColorSets[2]);
    }

    public void ChangePlayerColorToRed()
    {
        LevelManager.Instance.LevelGenerator.PlayerData.ChangeColor(CosmeticManager.Instance.ColorSets[2]);
        LevelManager.Instance.LevelGenerator.EnemiesInfo[0].ChangeColor(CosmeticManager.Instance.ColorSets[1]);
    }

    public void GoToStore()
    {
        UIManager.Instance.OpenUI(GlobalVariables.UIType.Store);
    }

    public void OnWinning(int gold)
    {
        GoldEarned = gold;

        PMainMenu.SetActive(false);
        PWon.SetActive(true);
        UpdateTextInfo(TxtGoldWinning, gold.ToString());
    }

    public void OnLosing(int gold)
    {
        GoldEarned = gold;
        PMainMenu.SetActive(false);
        PWon.SetActive(true);
        UpdateTextInfo(TxtGoldLosing, gold.ToString());
    }

    public void MultiplyGoldEarned()
    {
        int multiply = InversePendulum.GetMultiply();
        int gold = UpgradeSystem.Instance.GoldOfflineEarnings();
        GoldEarned = gold * multiply;
        UpdateTextInfo(TxtGoldOfflineEarnings, GoldEarned.ToString());
        UpdateTextInfo(TxtNoThanks, "CONTINUE");
    }

    public void ClaimGoldOfflineEarnings()
    {
        GoldEarned = UpgradeSystem.Instance.GoldOfflineEarnings(); 
        POfflineEarnings.SetActive(false);
        UpdateTextInfo(TxtGoldMainMenu, UpgradeSystem.Instance.AddGold(GoldEarned).ToString());
    }

    public void ClaimGoldAfterPlaying()
    {
        PMainMenu.SetActive(true);
        UpgradeSystem.Instance.AddGold(GoldEarned);
        Debug.Log(UpgradeSystem.Instance.UserData.UserStat.Gold);
        if (PWon.activeInHierarchy)
        {
            PWon.SetActive(false);
        }
        else if (PLose.activeInHierarchy)
        {
            PLose.SetActive(true);
        }
        OpenPanel();
    }

    public void UpdateTextInfo(TextMeshProUGUI textMesh, string info)
    {
        textMesh.text = info;
    }
}
