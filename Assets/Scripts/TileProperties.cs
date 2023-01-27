using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    private void Awake()
    {
        var rs = FindObjectOfType<RoomSpawns>();
        var enemies = GetComponentsInChildren<EnemyStats>();
        foreach (EnemyStats enemy in enemies)
        {
            enemy.maxHealth *= 1+((rs.maxLevelSize - rs.levelSize) * 0.1f);
            enemy.currentHealth = enemy.maxHealth;
            enemy.enemySoulLevel *= (int) (1 + ((rs.maxLevelSize - rs.levelSize) * 0.1f));
        }
    }
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
