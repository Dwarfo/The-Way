using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    public Button LiButton;
    public Button AStarButton;
    public Button NewGame;
    public Button QuitGame;
    public Button Save;
    public Button Menu;

    public Manager manager;
    public GameObject notifyAboutSaves;

    private Algorythms algo;

    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        algo = GameObject.Find("Path").GetComponent<Algorythms>();
        LiButton.onClick.AddListener(ShowPathLi);
        AStarButton.onClick.AddListener(ShowPathAStar);
        NewGame.onClick.AddListener(generateNewWorld);
        QuitGame.onClick.AddListener(quitGame);
        Save.onClick.AddListener(SaveGame);
        Menu.onClick.AddListener(LoadMenu);
    }

    void ShowPathLi()
    {
        refresh();
        algo.findPathLi(GameState.PlayerPosition);
    }

    void ShowPathAStar()
    {
        refresh();
        algo.findPathAStar(GameState.PlayerPosition);
    }

    void refresh()
    {
        foreach (Transform child in GameState.Path.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (GridTile tile in GameState.mapGrid)
            tile.resetPath();
    }

    void generateNewWorld()
    {
        manager.makeNewWorld();
    }

    void quitGame()
    {
        Application.Quit();
    }

    void SaveGame()
    {
        int i = 1;
        List<string> saves = GameState.getAllSaves(Application.persistentDataPath + "/");
        while (saves.Contains(Application.persistentDataPath + "/" + i.ToString() + ".s") && i < 5)
            i++;
        if (i != 5)
            GameState.saveGameState(GameState.mapGrid, i.ToString());
        else
            StartCoroutine(showNotifier());
    }

    //make it load menu
    void LoadMenu()
    {
        SceneManager.LoadScene("Generator", LoadSceneMode.Single);
        //GameState.loadGameState("/save.s");
        //manager.makeNewWorld(GameState.mapGrid);
    }

    IEnumerator showNotifier()
    {
        notifyAboutSaves.SetActive(true);
        yield return new WaitForSeconds(3);
        notifyAboutSaves.SetActive(false);
    }



}
