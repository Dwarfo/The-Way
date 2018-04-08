using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorythms : MonoBehaviour{


    List<GridTile> Li1 = new List<GridTile>();
    List<GridTile> Li2 = new List<GridTile>();
    bool gotDestination = false;
    GridTile Destination;
    public GameObject pathTile;
    public GameObject noWayInfo;

    public void findPathLi(GridTile start)
    {
        gotDestination = false;
        Li1.Clear();
        Li2.Clear();
        int pathNum = 2;
        start.path = 1;
        
        checkNeighboursLi(start, Li1, pathNum);

        while (!gotDestination)
        {
            pathNum++;
            if (pathNum % 2 != 0 && Li1.Count != 0)
            {
                Li2.Clear();
                foreach (GridTile tile in Li1)
                {
                    checkNeighboursLi(tile, Li2, pathNum);
                    
                }
            }

            if (pathNum % 2 == 0 && Li2.Count != 0)
            {
                Li1.Clear();
                foreach (GridTile tile in Li2)
                {
                    checkNeighboursLi(tile, Li1, pathNum);
                    
                }
            }

            if ((pathNum % 2 != 0 && Li1.Count == 0) || (pathNum % 2 == 0 && Li2.Count == 0))
            {
                Debug.Log("DID NOT FOUND IT");
                noWayInfo.SetActive(true);
                gotDestination = true;
            }
        }

        checkBackPathLi(Destination);
    }
    public void findPathAStar(GridTile start)
    {
        Li1.Clear();    //toCheck
        Li2.Clear();    //toIgnore
        gotDestination = false;
        int tries = 0;
        GridTile Current = start;
        Current.path = 1;
        Li1.Add(Current);

        while (Li1.Count > 0 && !gotDestination && tries < GameState.worldSize*GameState.worldSize)
        {
            tries++;
            checkNeighboursAStar(Current, start);
            if (Li1.Count > 0 && !gotDestination)
                Current = findMinimum(Li1);

        }
        if (gotDestination)
        {
            Debug.Log("FOUND IT path= " + Destination.path + " x: " + Destination.x + " y: " + Destination.y);
            Debug.Log("Number of Iterations: " + tries);
            checkBackPathLi(Destination);
        }
        else
        {
            noWayInfo.SetActive(true);
            Debug.Log("Cannot reach destination");
        }
    }


    private void checkNeighboursLi(GridTile tile, List<GridTile> Li, int pathNum)
    {
        if (tile.isDestination())
        {
            gotDestination = true;
            Destination = tile;
            Debug.Log("FOUND IT path= " + tile.path + " x: " + tile.x + " y: " + tile.y);
            Debug.Log("Number of tries: " + pathNum);
            return;

        }

        if (GameState.mapGrid[tile.x + 1, tile.y].path == 0)
        {
            Li.Add(GameState.mapGrid[tile.x + 1, tile.y]);
            GameState.mapGrid[tile.x + 1, tile.y].path = pathNum;
        }
        if (GameState.mapGrid[tile.x - 1, tile.y].path == 0)
        {
            Li.Add(GameState.mapGrid[tile.x - 1, tile.y]);
            GameState.mapGrid[tile.x - 1, tile.y].path = pathNum;
        }
        if (GameState.mapGrid[tile.x, tile.y + 1].path == 0)
        {
            Li.Add(GameState.mapGrid[tile.x, tile.y + 1]);
            GameState.mapGrid[tile.x, tile.y + 1].path = pathNum;
        }
        if (GameState.mapGrid[tile.x, tile.y - 1].path == 0)
        {
            Li.Add(GameState.mapGrid[tile.x, tile.y - 1]);
            GameState.mapGrid[tile.x, tile.y - 1].path = pathNum;
        }
    }
    private void checkBackPathLi(GridTile start)
    {
        GridTile tile = start;
        Instantiate(pathTile, new Vector2(tile.x * (2.5f) + 1.25f, tile.y * (2.5f) + 1.25f), Quaternion.identity).transform.SetParent(GameState.Path.transform, false);
        while (tile.path != 1)
        {
            if (GameState.mapGrid[tile.x + 1, tile.y].path == (tile.path - 1))
            {
                tile = GameState.mapGrid[tile.x + 1, tile.y];
                Instantiate(pathTile, new Vector2(tile.x * (2.5f) + 1.25f, tile.y * (2.5f) + 1.25f), Quaternion.identity).transform.SetParent(GameState.Path.transform, false);

            }
            if (GameState.mapGrid[tile.x - 1, tile.y].path == (tile.path - 1))
            {
                tile = GameState.mapGrid[tile.x - 1, tile.y];
                Instantiate(pathTile, new Vector2(tile.x * (2.5f) + 1.25f, tile.y * (2.5f) + 1.25f), Quaternion.identity).transform.SetParent(GameState.Path.transform, false);
            }
            if (GameState.mapGrid[tile.x, tile.y + 1].path == (tile.path - 1))
            {
                tile = GameState.mapGrid[tile.x, tile.y + 1];
                Instantiate(pathTile, new Vector2(tile.x * (2.5f) + 1.25f, tile.y * (2.5f) + 1.25f), Quaternion.identity).transform.SetParent(GameState.Path.transform, false);
            }
            if (GameState.mapGrid[tile.x, tile.y - 1].path == (tile.path - 1))
            {
                tile = GameState.mapGrid[tile.x, tile.y - 1];
                Instantiate(pathTile, new Vector2(tile.x * (2.5f) + 1.25f, tile.y * (2.5f) + 1.25f), Quaternion.identity).transform.SetParent(GameState.Path.transform, false);
            }
        }
    }
    private void checkNeighboursAStar(GridTile tile, GridTile start)
    {
        

        checkTile(GameState.mapGrid[tile.x + 1, tile.y], start, tile.path + 1 );
        checkTile(GameState.mapGrid[tile.x - 1, tile.y], start, tile.path + 1);
        checkTile(GameState.mapGrid[tile.x, tile.y + 1], start, tile.path + 1);
        checkTile(GameState.mapGrid[tile.x, tile.y - 1], start, tile.path + 1);

        Li1.Remove(tile);
        Li2.Add(tile);

    }
    private GridTile findMinimum(List<GridTile> list)
    {
        GridTile minimum = list[0];

        foreach (GridTile newMinimum in list)
        {
            if ((newMinimum.H + newMinimum.path) < (minimum.H + minimum.path))
                minimum = newMinimum;
        }
        Debug.Log(minimum.H + minimum.path);
        return minimum;



    }
    private void checkTile(GridTile tile, GridTile start, int pathNum)
    {
        if (!tile.isOccupied() && !Li2.Contains(tile))
        {
            if (!Li1.Contains(tile))
            {
                tile.path = pathNum;
                Li1.Add(tile);
            }
            else if (tile.path + tile.H > tile.H + pathNum)
                tile.path = pathNum;
            
        }

        if (tile.isDestination())
        {
            Destination = tile;
            Li1.Clear();
            gotDestination = true;

        }

    }

}
