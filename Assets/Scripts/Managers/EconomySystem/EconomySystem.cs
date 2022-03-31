using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomySystem : Singleton<EconomySystem>
{
    private UserData userData;
    private Player player;


    private void Awake()
    {
        userData = JSONSaving.Instance.UserData;
        player = LevelManager.Instance.Player;
    }

    public void WinningGoldEarned(int levelID)
    {
        player.OwnerStat.Currency = Formula.WinningGoldEarned(levelID);
    }

    public void LoseGoldEarned(int levelID)
    {
        player.OwnerStat.Currency = Formula.LoseGoldEarned(levelID);
    }

    public static void GoldWinningOffline()
    {

    }
}
