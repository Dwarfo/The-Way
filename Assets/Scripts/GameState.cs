using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GameState  {

    public static float tileSize = 2.5f;
    public static int worldSize = 10;
    public static GridTile[,] mapGrid;
    public static GameObject Path;
    public static bool gameOver = false;

    public static GridTile PlayerPosition;

    public static void saveGameState(GridTile[,] grid, string savename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.Create(Application.persistentDataPath + "/" + savename + ".s");

        bf.Serialize(stream, new gameSaveData(mapGrid));
        stream.Close();
    }

    public static void loadGameState(string savename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.OpenRead(Application.persistentDataPath + savename);

        gameSaveData gsd = bf.Deserialize(stream) as gameSaveData;
        mapGrid = gsd.mapGrid;
        stream.Close();

    }

    public static List<string> getAllSaves(string path)
    {
        return new List<string>(Directory.GetFiles(path));
    }

    public static void DeleteSave(int save)
    {
        string path = Application.persistentDataPath + "/" + save.ToString() + ".s";
        if (File.Exists(path))
            File.Delete(path);
    }

}


[System.Serializable]
public class gameSaveData {

    public GridTile[,] mapGrid;

    public gameSaveData(GridTile[,] mapGrid)
    {
        this.mapGrid = mapGrid;
    }

}