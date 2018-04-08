using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaveButton : MonoBehaviour {

    public int saveNum;
    public Button deleteButton;
    private Button self;


	void Start ()
    {
        self = gameObject.GetComponent<Button>();
        deleteButton.onClick.AddListener(Delete);
        self.onClick.AddListener(LoadGame);

        List<string> saves = GameState.getAllSaves(Application.persistentDataPath + "/");
        //Debug.Log(saveNum.ToString() + ".s");
        //Debug.Log(saves[0]);
        if (!saves.Contains(Application.persistentDataPath + "/" + saveNum.ToString() + ".s"))
            gameObject.SetActive(false);
    }

    private void LoadGame()
    {
        GameState.loadGameState("/" + saveNum.ToString() + ".s");
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Single);
    }

    private void Delete()
    {
        GameState.DeleteSave(saveNum);
        gameObject.SetActive(false);
    }
}
