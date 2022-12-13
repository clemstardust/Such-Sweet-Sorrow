using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadEnemies : MonoBehaviour
{
    public void Start()
    {
        NavMeshAgent[] enemies = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent enemy in enemies)
        {
            enemy.enabled = true;
        }
    }
}
