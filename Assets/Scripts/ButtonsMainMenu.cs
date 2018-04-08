using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonsMainMenu : MonoBehaviour {

    public GameObject loadMenu;
    public GameObject mainMenu;
    public InputField worldSize;

    public Button LoadButton;
    public Button QuitButton;
    public Button NewGameButton;
    public Button BackButton;

    private void Start()
    {
        LoadButton.onClick.AddListener(LoadGame);
        QuitButton.onClick.AddListener(quitGame);
        NewGameButton.onClick.AddListener(newGame);
        BackButton.onClick.AddListener(back);
    }


    void quitGame()
    {
        Application.Quit();
    }

    void LoadGame()
    {
        loadMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    void back()
    {
        mainMenu.SetActive(true);
        loadMenu.SetActive(false);
    }
    void newGame()
    {
        int size;
        if (int.TryParse(worldSize.text, out size))
        {
            if (size >= 10)
            {
                GameState.worldSize = size;
                GameState.mapGrid = null;
                SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
            }
            else
                worldSize.text = "Try more than 10";
        }
        else
            worldSize.text = "Please type whole number";
    }
}
