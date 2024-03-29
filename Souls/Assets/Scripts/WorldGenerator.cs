﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public Chunk[] cityTiles;
    public Chunk[] forestTiles;
    private GameObject[] chunks = new GameObject[areaSize * areaSize];

    private GameObject player;

    const int chunkSize = 32;
    const int areaSize = 11;
    public const int playerSpawn = areaSize / 2;
    //const int worldSize = 32;

    private void Start()
    {
        generateCity(0, 0, 8, 8);
        Debug.Log("Level generated!");
        player = GameObject.Find("PlayerObject");
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            //chunks[i].SetActive(true); //DEACTIVATE THIS
            if ((chunks[i].transform.position - player.transform.position).sqrMagnitude < 1500f)
            {
                chunks[i].SetActive(true);
            }
            else
            {
                chunks[i].SetActive(false);
            }
        }
    }

    int[][] sideCoords = new int[4][]
    {
        new int[2] {1, 0},
        new int[2] {0, 1},
        new int[2] {-1, 0},
        new int[2] {0, -1},
    };

    void generateCity(int x, int y, int enterX, int enterY)
    {
        int[,] map = new int[areaSize, areaSize];
        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                map[i, j] = -1;
            }
        }
        List<int> openList = new List<int>();

        int half = areaSize / 2;

        map[half, half + 1] = 17;
        map[half, half] = 16;

        map[half, half + 3] = 16;
        map[half, half - 3] = 16;
        map[half + 3, half] = 16;
        map[half - 3, half] = 16;

        map[1, 1] = 16;
        map[1, areaSize - 2] = 16;
        map[areaSize - 2, 1] = 16;
        map[areaSize - 2, areaSize - 2] = 16;

        openList.Add(half - 1 + half * areaSize);
        openList.Add(half + (half - 1) * areaSize);
        openList.Add(half + 1 + half * areaSize);

        while (openList.Count > 0)
        {
            int index = Random.Range(0, openList.Count);
            int current = openList[index];

            int x0 = current % areaSize;
            int y0 = current / areaSize;

            map[x0, y0] = 0;

            List<int> existingSides = new List<int>();

            for(int i = 0; i < 4; i++)
            {
                int x1 = x0 + sideCoords[i][0];
                int y1 = y0 + sideCoords[i][1];
                //Debug.Log("" + x1 + ", " + y1);

                if (isInBounds(x1, y1))
                {
                    if(map[x1, y1] == -1)
                    {
                        if (!openList.Contains(x1 + y1 * areaSize))
                        {
                            openList.Add(x1 + y1 * areaSize);
                            //Debug.Log("Added to open list");
                        }
                    }
                    else if(map[x1, y1] != 17)
                    {
                        existingSides.Add(i);
                        //Debug.Log("Added to side list");
                    }
                }
            }

            if(existingSides.Count > 0)
            {
                for(int i = 0; i < existingSides.Count; i++)
                {
                    int side = existingSides[i];
                    int x1 = x0 + sideCoords[side][0];
                    int y1 = y0 + sideCoords[side][1];
                    if(map[x1, y1] == 16)
                    {
                        map[x0, y0] |= (1 << side);
                        existingSides.RemoveAt(i);
                    }
                }
                if(existingSides.Count > 0)
                {
                    int r = Random.Range(0, existingSides.Count);
                    int side = existingSides[r];
                    int x1 = x0 + sideCoords[side][0];
                    int y1 = y0 + sideCoords[side][1];

                    map[x0, y0] |= (1 << side);
                    map[x1, y1] |= (1 << ((side + 2) % 4));
                    existingSides.RemoveAt(r);
                }
                if(existingSides.Count > 0 && Random.value > 0.8f)
                {
                    int r = Random.Range(0, existingSides.Count);
                    int side = existingSides[r];
                    int x1 = x0 + sideCoords[side][0];
                    int y1 = y0 + sideCoords[side][1];

                    map[x0, y0] |= (1 << side);
                    map[x1, y1] |= (1 << ((side + 2) % 4));
                }
                //Debug.Log("connected!");
            }
            else
            {
                //Debug.Log("can't connect!");
            }
            openList.RemoveAt(index);
        }

        //Debug.LogError("");

        //generating finished, create chunks
        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                if(map[i, j] == -1)
                {
                    Debug.Log("-1 @ " + i + ", " + j + "error!");
                    continue;
                }
                if (map[i, j] == -1)
                {
                    Debug.Log("0 error!");
                    continue;
                }
                // find tile type & orientation
                // loop through all rotations
                int data = map[i, j];
                bool success = false;
                for(int k = 0; k < 4; k++)
                {
                    //Debug.Log(
                    //    (((data & 8) > 0) ? "1" : "0") + " " +
                    //    (((data & 4) > 0) ? "1" : "0") + " " +
                    //    (((data & 2) > 0) ? "1" : "0") + " " + 
                    //    (((data & 1) > 0) ? "1" : "0")
                    //    );

                    if (data == 16)
                    {
                        CreateTile(i, j, 0, Chunk.Type.Market);
                        success = true;
                        break;
                    }
                    if (data == 17)
                    {
                        CreateTile(i, j, 0, Chunk.Type.Boss);
                        success = true;
                        break;
                    }
                    if (data == 15)
                    {
                        CreateTile(i, j, Random.Range(0, 4), Chunk.Type.FourWay);
                        success = true;
                        break;
                    }
                    if (data == 7)
                    {
                        CreateTile(i, j, k, Chunk.Type.TIntersection);
                        success = true;
                        break;
                    }
                    if (data == 3)
                    {
                        CreateTile(i, j, k, Chunk.Type.Corner);
                        success = true;
                        break;
                    }
                    if (data == 5)
                    {
                        CreateTile(i, j, (k + (Random.value > 0.5f ? 2 : 0)) % 4, Chunk.Type.Corridor);
                        success = true;
                        break;
                    }
                    if (data == 1)
                    {
                        CreateTile(i, j, k, Chunk.Type.End);
                        success = true;
                        break;
                    }

                    data = data << 1;
                    data += ((data & 16) == 16) ? 1 : 0;
                    data &= 15;
                }
                if(!success)
                {
                    Debug.LogError("!Success");
                }
            }
        }
    }

    bool isInBounds(int x, int y)
    {
        return x < areaSize && y < areaSize && x >= 0 && y >= 0;
    }

    void CreateTile(int x, int y, int rotation, Chunk.Type type)
    {
        rotation = 4 - rotation;
        List<Chunk> accepted = new List<Chunk>();
        Chunk[] tiles;
        int half = areaSize / 2;
        if((Mathf.Abs(x - half) < 3 || Mathf.Abs(y - half) < 3) && (Mathf.Abs(x - half) > 3 || Mathf.Abs(y - half) > 3))
        {
            tiles = forestTiles;
        }
        else
        {
            tiles = cityTiles;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].type == type)
                accepted.Add(tiles[i]);
        }
        Chunk theChosenOne = accepted[Random.Range(0, accepted.Count)];

        GameObject chunk = Instantiate(theChosenOne.gameObject);
        chunk.transform.position = new Vector3(x * chunkSize, y * chunkSize);
        chunk.transform.eulerAngles = new Vector3(0f, 0f, 90f * rotation);
        chunks[x + y * areaSize] = chunk;
        chunk.SetActive(false);
    }
}
