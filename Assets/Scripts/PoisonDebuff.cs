using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : MonoBehaviour
{
    public static IEnumerator Poison_Debuff(EnemyStats enemyStats, float damage)
    {
        while (enemyStats.currentHealth > 0)
        {
            enemyStats.LiterallyJustTakeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }
}
