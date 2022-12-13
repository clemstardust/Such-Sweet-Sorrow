using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Information")]
    //public Sprite itemIcon;
    public string itemName;
    public string itemDesc;
    public bool unlocked;
}
