using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class UIController : MonoBehaviour
{
    public static bool passedFirstLevel = false;

    public Canvas Canvas;
    public List<UIElement> UIElementList;
    public UIElement Gameplay;
    public UIMainMenu MainMenu;

    private LevelGenerator levelGenerator;

   

    private void Start()
    {
        GetListUIElement();
        MainMenu = (UIMainMenu) UIElementList.Find(x => x.Type == GlobalVariables.UIType.MainMenu);       
        levelGenerator = LevelManager.Instance.LevelGenerator; 
    }
    public void GetListUIElement()
    {
        UIElementList = Canvas.GetComponentsInChildren<UIElement>().ToList();
    }

    public void ShowMainMenu()
    {
        if (!passedFirstLevel)
        {
            ShowUI(MainMenu.ColorPicker, true);
            ShowUI(MainMenu.Upgrade, false);
        }
        else
        {
            ShowUI(MainMenu.ColorPicker, false);
            ShowUI(MainMenu.Upgrade, true);
        }
    }

    public void TapToPlay()
    {
        GameManager.Instance.SwitchState(GameState.GameStart);
        levelGenerator.EnableGenerateFighter();
        ShowUI(Gameplay.gameObject, true);
        ShowUI(MainMenu.gameObject, false);
    }

    public void BackToMainMenu()
    {
        
        GameManager.Instance.SwitchState(GameState.MainMenu);
        ObjectPooler.Instance.DeSpawnAllFighters();
        levelGenerator.SetBuildingToDefault();
        ShowUI(Gameplay.gameObject, false);
        ShowUI(MainMenu.gameObject, true);
    }

    public void ChangePlayerColorToRed()
    {
        levelGenerator.ChangePlayerColorToRed();
    }

    public void ChangePlayerColorToBlue()
    {
        levelGenerator.ChangePlayerColorToBlue();
    }

    public void ShowUI(GameObject uiElement, bool isShow)
    {
        uiElement.SetActive(isShow);
    }
}
