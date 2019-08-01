using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Souls/Chunk")]
public class Chunk : ScriptableObject
{
    public int sides; // Right = 0, counter-clockwise powers of two
    public float randomWeight = 1.0f; // How often should this chunk appear
    public GameObject tilemapPrefab;
}
