using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public GlobalVariables.UpgradeType Type;
    public UpgradeData UpgradeData;
    public TextMeshProUGUI UpgradeNumber;
    public TextMeshProUGUI CurrentLevel;
    public TextMeshProUGUI UpgradeCost;

    public void Start()
    {
        ShowUpgradeData();
    }

    public void ShowUpgradeData()
    {
        UpgradeNumber.text = UpgradeData.Value.ToString();
        CurrentLevel.text = "LVL " + UpgradeData.Level.ToString();
        UpgradeCost.text = UpgradeData.Cost.ToString();
    }
}
