using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTDebuff : MonoBehaviour
{
    public static IEnumerator DOT_Debuff(EnemyStats enemyStats, float damage, float duration, EnemyDebuffHandler debuffHandler)
    {
        float damagePerTick = damage / duration;
        debuffHandler.darkStacks++;
        while (damage >= 0)
        {
            enemyStats.TakeDOTHit((int)damagePerTick);
            damage -= damagePerTick;
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }
}
