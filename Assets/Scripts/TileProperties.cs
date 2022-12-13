using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [SerializeField] public enum Type
    {
        hallway,
        mainroom,
        spawn,
        special,
        boss
    };
    public Type chosenType; 
    [SerializeField] public enum LeadsTo
    {
        hallway,
        mainroom,
        boss,
        special,
        any
    };
    public LeadsTo leadsTo;
    [SerializeField] public enum Theme
    {
        dungeon
    };
    public Theme theme;
}
