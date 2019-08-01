using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Chunk[] tiles;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
            for(int j = 0; j < 4; j++)
            {
                Instantiate(tiles[Random.Range(0, tiles.Length)].tilemapPrefab).transform.position = new Vector3(i * 16, j * 16);
            }
    }
}
