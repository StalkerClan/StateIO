using UnityEngine;

public class UIMainMenu : UICanvas
{
    public GameObject PColorSwitcher;
    public GameObject PUpgrades;

    private void Start()
    {
       
    }

    public override void OnInit()
    {
        base.OnInit();
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
        LevelManager.Instance.LevelGenerator.PlayerData.ChangeColor(UltilitiesManager.Instance.ColorSets[1]);
        LevelManager.Instance.LevelGenerator.EnemiesInfo[0].ChangeColor(UltilitiesManager.Instance.ColorSets[2]);
    }

    public void ChangePlayerColorToRed()
    {
        LevelManager.Instance.LevelGenerator.PlayerData.ChangeColor(UltilitiesManager.Instance.ColorSets[2]);
        LevelManager.Instance.LevelGenerator.EnemiesInfo[0].ChangeColor(UltilitiesManager.Instance.ColorSets[1]);
    }
  
}
