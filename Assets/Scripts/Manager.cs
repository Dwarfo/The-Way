using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public int size;
    public GameObject backGroundTile;
    public GameObject ObstacleTile;
    public GameObject Player;
    public GameObject Rarity;
    public GameObject Path;
    public CameraControlls camera;
    public GameObject GameOverSign;
    public GameObject GameOverMenu;
    public GameObject noWayInfo;
    public GameObject redInfo;
    public GameObject blueInfo;


    private PlayerController pc;
    private GameObject DestinationRef;

    void Start ()
    {
        StartCoroutine(hideInfo());
        GameState.Path = GameObject.Find("Path");
        if (GameState.mapGrid == null)
            makeNewWorld();
        else
        {
            makeNewWorld(GameState.mapGrid);
        }

        showGrid();

    }

    public void makeNewWorld()
    {
        if (pc != null)
        {
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in GameState.Path.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        GameState.gameOver = false;
        GameOverMenu.SetActive(false);
        GameOverSign.SetActive(false);
        size = GameState.worldSize;
        GameState.mapGrid = new GridTile[size, size];

        int k = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                CreateBackgroundTile(i, j, k);
                GameState.mapGrid[i, j] = new GridTile(i, j);
                k++;
            }
        }

        CreateBounds();
        PopulateWithObjects();
        setDestination();
        spawnPlayer();
        DrawObjects();
        camera.player = pc.transform;
    }

    public void makeNewWorld(GridTile [,] gridMap)
    {
        if (pc != null)
        {
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in GameState.Path.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        GameState.gameOver = false;
        GameOverMenu.SetActive(false);
        GameOverSign.SetActive(false);
        size = GameState.worldSize;
        GameState.mapGrid = gridMap;

        int k = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                CreateBackgroundTile(i, j, k);
                k++;
            }
        }

        DrawObjects();
        camera.player = pc.transform;
    }

    private void PopulateWithObjects()
    {
        int toSpawn;

        for (int i = 1; i < size - 1; i = i + 2)
        {
            for (int j = 2; j < size - 1; j = j + 2)
            {
                toSpawn = UnityEngine.Random.Range(0, 10);
                if (toSpawn >= 6)
                {
                    makeObstaclePositions(i, j, GameState.mapGrid);
                }
            }
        }
    }

    private void DrawObjects()
    {
        foreach (GridTile tile in GameState.mapGrid)
        {
            if (tile.isOccupied())
                Instantiate(ObstacleTile, new Vector2(tile.x * (GameState.tileSize) + 1.25f, tile.y * (GameState.tileSize) + 1.25f), Quaternion.identity).transform.SetParent(transform, false);
            if (tile.isDestination())
            {
                DestinationRef = Instantiate(Rarity, new Vector2(tile.x * (GameState.tileSize) + 1.25f, tile.y * (GameState.tileSize) + 1.25f), Quaternion.identity);
                DestinationRef.transform.SetParent(transform, false);

            }
            if (tile.isPlayerPosition())
            {
                pc = Instantiate(Player, new Vector2(tile.x * (GameState.tileSize) + 1.25f, tile.y * (GameState.tileSize) + 1.25f), Quaternion.identity).GetComponent<PlayerController>();
                pc.setPosition(tile.x, tile.y);
                pc.gameOverMenu = GameOverMenu;
                pc.gameOverSign = GameOverSign;
                pc.transform.SetParent(transform, false);
                GameState.PlayerPosition = GameState.mapGrid[tile.x, tile.y];
            }
        }
    }

    private void CreateBackgroundTile(int i, int j, int k)
    {
        GameObject go = Instantiate(backGroundTile, new Vector2(i * (GameState.tileSize) + 1.25f, j * (GameState.tileSize) + 1.25f), Quaternion.identity);
        go.name = "Background" + (k).ToString();
        go.transform.SetParent(transform, false);
    }

    private void CreateBounds()
    {
        for (int i = 0; i < size; i++)
        {
            GameState.mapGrid[0, i].Occupy();
            GameState.mapGrid[i, 0].Occupy();
            GameState.mapGrid[i, size - 1].Occupy();
            GameState.mapGrid[size - 1, i].Occupy();
        }
    }

    void showGrid()
    {
        string row;
        Debug.Log("Obstacles:");
        for (int i = 0; i < size; i++)
        {
            row = "";
            for (int j = 0; j < size; j++)
            {
                row += GameState.mapGrid[j, i].gridOccupied();
            }
            Debug.Log(row);
        }
        
        Debug.Log("Path:");
        for (int i = 0; i < size; i++)
        {
            row = "";
            for (int j = 0; j < size; j++)
            {
                row += GameState.mapGrid[j, i].path + " ";
            }
            Debug.Log(row);
        }
        
        Debug.Log("Heuristics:");
        for (int i = 0; i < size; i++)
        {
            row = "";
            for (int j = 0; j < size; j++)
            {
                row += GameState.mapGrid[j, i].H + " ";
            }
            Debug.Log(row +" :" + i);
        }
    }

    private void setDestination()
    {
        int x;
        int y;

        do
        {
            x = UnityEngine.Random.Range(2, size);
            y = UnityEngine.Random.Range(2, size);

        } while (GameState.mapGrid[x, y].isOccupied());

        GameState.mapGrid[x, y].setDestination();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Heuristic(GameState.mapGrid[i, j], GameState.mapGrid[x, y]);
            }
        }

    }

    private void spawnPlayer()
    {
        int x;
        int y;

        do
        {
            x = UnityEngine.Random.Range(2, size);
            y = UnityEngine.Random.Range(2, size);

        } while (GameState.mapGrid[x, y].isOccupied() || GameState.mapGrid[x, y].isDestination());

        GameState.mapGrid[x, y].setPlayerPos();
        GameState.PlayerPosition = GameState.mapGrid[x, y];

        
    }

    private void Heuristic(GridTile tile, GridTile destination)
    {
        tile.H = 2* (Math.Abs(destination.x - tile.x) + Math.Abs(destination.y - tile.y)); 
    }

    private void makeObstaclePositions(int i, int j, GridTile[,] mapGrid)
    {
        int obst = UnityEngine.Random.Range(1, 10);

        switch (obst)
        {
            case 1:
                mapGrid[i, j].Occupy();
                break;
            case 2:
                mapGrid[i + 1, j].Occupy();
                break;
            case 3:
                mapGrid[i, j + 1].Occupy();
                break;
            case 4:
                mapGrid[i + 1, j + 1].Occupy();
                break;
            case 5:
                mapGrid[i, j].Occupy();
                mapGrid[i + 1, j].Occupy();
                break;
            case 6:
                mapGrid[i, j + 1].Occupy();
                mapGrid[i + 1, j + 1].Occupy();
                break;
            case 7:
                mapGrid[i, j].Occupy();
                mapGrid[i, j + 1].Occupy();
                break;
            case 8:
                mapGrid[i + 1, j].Occupy();
                mapGrid[i + 1, j + 1].Occupy();
                break;
            case 9:
                mapGrid[i + 1, j].Occupy();
                mapGrid[i + 1, j + 1].Occupy();
                mapGrid[i, j].Occupy();
                mapGrid[i, j + 1].Occupy();
                break;
        }

    }

    IEnumerator hideInfo()
    {
        noWayInfo.SetActive(false);
        yield return new WaitForSeconds(8);
        redInfo.SetActive(false);
        blueInfo.SetActive(false);

    }
}
