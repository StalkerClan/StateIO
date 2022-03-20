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
        SetUpgradeData();
    }

    public void SetUpgradeData()
    {
        UpgradeNumber.text = UpgradeData.BaseValue.ToString();
        CurrentLevel.text = "LVL " + UpgradeData.Level.ToString();
        UpgradeCost.text = UpgradeData.Cost.ToString();
    }
}
