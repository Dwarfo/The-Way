using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	public PlayerController pc;

    void Start ()
    {
        pc = gameObject.GetComponent<PlayerController>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!GameState.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                pc.movement(new Vector2Int(0, 1), 0);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pc.movement(new Vector2Int(-1, 0), 90);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                pc.movement(new Vector2Int(0, -1), 180);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                pc.movement(new Vector2Int(1, 0), -90);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pc.gameOverMenu.SetActive(!pc.gameOverMenu.active);
                
            }
        }
    }
}
