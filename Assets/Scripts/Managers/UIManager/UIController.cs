using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class UIController : MonoBehaviour
{
    public static bool passedFirstLevel = false;

    public Canvas Canvas;
    public List<UIElement> UIElementList;
    public UIMainMenu MainMenu;
    public UIElement Gameplay;

    private LevelGenerator levelGenerator;

    private void Start()
    {
        GetListUIElement();
        MainMenu = (UIMainMenu) UIElementList.Find(x => x.Type == GlobalVariables.UIType.MainMenu);       
        Gameplay = UIElementList.Find(x => x.Type == GlobalVariables.UIType.Gameplay);
        ShowUI(Gameplay.gameObject, false);
        levelGenerator = LevelManager.Instance.LevelGenerator; 
    }
    public void GetListUIElement()
    {
        UIElementList = Canvas.GetComponentsInChildren<UIElement>().ToList();
    }

    public void ShowMainMenu()
    {
        ShowUI(Gameplay.gameObject, false);
        ShowUI(MainMenu.gameObject, true);
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
    }

    public void BackToMainMenu()
    {       
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }

    public void ShowGameplay()
    {
        ShowUI(Gameplay.gameObject, true);
        ShowUI(MainMenu.gameObject, false);
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
