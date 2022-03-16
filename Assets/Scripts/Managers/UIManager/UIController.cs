using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class UIController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ColorPickerPanel;
    public GameObject UpgradePanel;
    public GameObject Gameplay;
    private LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = LevelManager.Instance.LevelGenerator;
        if (LevelManager.Instance.LevelID >= 2)
        {
            ShowUI(UpgradePanel, true);
            ShowUI(ColorPickerPanel, false);
        }
        else
        {
            ShowUI(UpgradePanel, false);
            ShowUI(ColorPickerPanel, true);
        }
    }

    public void TapToPlay()
    {
        ShowUI(MainMenu, false);
        ShowUI(Gameplay, true);
        GameManager.Instance.SwitchState(GameState.GameStart);
        levelGenerator.EnableGenerateFighter();     
    }

    public void BackToMainMenu()
    {
        ShowUI(MainMenu, true);
        ShowUI(Gameplay, false);
        GameManager.Instance.SwitchState(GameState.MainMenu);
        ObjectPooler.Instance.DeSpawnAllFighters();
        levelGenerator.SetBuildingToDefault();
    }

    public void ChangePlayerColorToRed()
    {
        levelGenerator.ChangePlayerColorToRed();
    }

    public void ChangePlayerColorToBlue()
    {
        levelGenerator.ChangePlayerColorToBlue();
    }

    private void ShowUI(GameObject uiElement, bool isShow)
    {
        uiElement.SetActive(isShow);
    }
}
