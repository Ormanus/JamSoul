﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public enum Type
    {
        FourWay,
        TIntersection,
        Corridor,
        Corner,
        End,
        Boss,
        Market
    }

    public enum Palette
    {
        City,
        Forest,
    }

    public Type type;
    public Palette palette;
    public float randomWeight = 1.0f; // How often should this chunk appear
}
