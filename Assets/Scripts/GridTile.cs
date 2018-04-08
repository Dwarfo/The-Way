using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridTile  {

    public int x;
    public int y;

    public int path = 0;
    public int H;
    private bool obstacle = false;
    private bool playerStart = false;
    private bool Destination = false;

    public GridTile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    public void resetPath()
    {
        if(!obstacle)
            path = 0;

    }

    public void setDestination()
    {
        Destination = !Destination;
    }

    public void setPlayerPos()
    {
        playerStart = !playerStart;
    }

    public void Occupy()
    {
        obstacle = true;
        path = -1;
    }

    public string gridOccupied()
    {
        if (obstacle)
            return "1";
        else
            return "0";
    }

    public bool isDestination()
    {
        return Destination;
    }

    public bool isOccupied()
    {
        return obstacle;
    }

    public bool isPlayerPosition()
    {
        return playerStart;
    }



}
