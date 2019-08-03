using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Chunk[] tiles;

    const int chunkSize = 16;
    const int areaSize = 16;
    const int worldSize = 16;

    private void Start()
    {
        //TODO: multiple areas, tilesets, connections between areas?

        //for (int i = 0; i < worldSize; i++)
        //{
        //    for (int j = 0; j < worldSize; j++)
        //    {

        //    }
        //}

        generateCity(0, 0, 8, 8);
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

        //if(x == 0 && y == 0)
        //{
        //    map[8, 9] = 16; 
        //}

        List<int> openList = new List<int>();
        openList.Add(enterX + enterY * areaSize);
        map[enterX, enterY] = 0;

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
                    else
                    {
                        existingSides.Add(i);
                        //Debug.Log("Added to side list");
                    }
                }
            }

            if(existingSides.Count > 0)
            {
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

                    if (data == 15)
                    {
                        CreateTile(i, j, k, Chunk.Type.FourWay);
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
                        CreateTile(i, j, k, Chunk.Type.Corridor);
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
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].type == type)
                accepted.Add(tiles[i]);
        }
        Chunk theChosenOne = accepted[Random.Range(0, accepted.Count)];

        GameObject chunk = Instantiate(theChosenOne.tilemapPrefab);
        chunk.transform.position = new Vector3(x * chunkSize, y * chunkSize);
        chunk.transform.eulerAngles = new Vector3(0f, 0f, 90f * rotation);
    }
}
