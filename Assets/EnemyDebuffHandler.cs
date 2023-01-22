using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuffHandler : MonoBehaviour
{
    public int poisonStacks = 0;
    public int iceStacks = 0;
    public int bleedStacks = 0;
    public int rotStacks = 0;
    public int hemmorageStacks = 0;
    public int stunStacks = 0;
    public int darkStacks = 0;

    private EnemyStats enemyStats;
    private EnemyAI enemyAI;
    private EnemyManager enemyManager;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyAI = GetComponent<EnemyAI>();
        enemyManager = GetComponent<EnemyManager>();
    }
    private void FixedUpdate()
    {
        if (poisonStacks > 0) enemyStats.LiterallyJustTakeDamage(12 * poisonStacks * Time.deltaTime);
        enemyAI.animator.speed = 1 - (0.1f * iceStacks);
        if (enemyManager.enemyMode == EnemyManager.Mode.dead)
        {
            enemyAI.animator.speed = 1;
        }
    }
}
