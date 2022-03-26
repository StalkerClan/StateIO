using UnityEngine;

public class UIMainMenu : UICanvas
{
    public GameObject PColorSwitcher;
    public GameObject PUpgrades;

    public void OpenPanel()
    {
        if (JSONSaving.Instance.UserData.Level > 1)
        {
            PColorSwitcher.SetActive(false);
            PUpgrades.SetActive(true);
        }
        else
        {
            PColorSwitcher.SetActive(true);
            PUpgrades.SetActive(false);
        }
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
}
