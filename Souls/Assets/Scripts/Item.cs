using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Custom/Item")]
public class Item : ScriptableObject
{
    public int price;
    public Sprite sprite;
    public string description;
    public int type;
}
