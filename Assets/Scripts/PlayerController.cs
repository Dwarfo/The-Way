using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gameOverMenu;
    public GameObject gameOverSign;
    public int numberOfSteps = 0;
    public Vector2Int position = new Vector2Int(1,1);

    public bool checkForObsacles(Vector2Int tile)
    {
        return GameState.mapGrid[tile.x, tile.y].isOccupied();      
    }

    public bool checkIfDestination(Vector2Int tile)
    {
        return GameState.mapGrid[tile.x, tile.y].isDestination();
    }

    public void movement(Vector2Int move, float rot)
    {
        Vector2Int tileToCheck = new Vector2Int(position.x + move.x, position.y + move.y);
        if (!checkForObsacles(tileToCheck))
        {
            position += move;
            numberOfSteps++;
            transform.position = new Vector2(position.x * (GameState.tileSize) + 1.25f, position.y * (GameState.tileSize) + 1.25f);
            transform.rotation = Quaternion.Euler(0, 0, rot);
            GameState.PlayerPosition = GameState.mapGrid[position.x, position.y];
            writePosition();
        }

        if (checkIfDestination(tileToCheck))
        {
            // Game won/restart
            Debug.Log("You Won!!");
            GameState.gameOver = true;
            gameOverMenu.SetActive(true);
            gameOverSign.SetActive(true);
        }
    }

    public void setPosition(int x, int y)
    {
        position.x = x;
        position.y = y;

        transform.position = new Vector2(position.x * (GameState.tileSize) + 1.25f, position.y * (GameState.tileSize) + 1.25f);
    }

    public void writePosition()
    {
        Debug.Log("Position: " + position);
    }

}

