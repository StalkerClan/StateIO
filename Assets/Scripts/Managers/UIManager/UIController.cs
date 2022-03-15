using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public  class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private LevelGenerator levelGenerator;

    public GameObject MainMenu { get => mainMenu; set => mainMenu = value; }
    public GameObject Gameplay { get => gameplay; set => gameplay = value; }
    public GameObject StartButton { get => startButton; set => startButton = value; }

    private void Start()
    {
        levelGenerator = LevelManager.Instance.LevelGenerator;
    }

    public void TapToPlay()
    {
        GameManager.Instance.SwitchState(GameState.GameStart);
        levelGenerator.EnableGenerateFighter();     
    }

    public void BackToMainMenu()
    {
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
}
