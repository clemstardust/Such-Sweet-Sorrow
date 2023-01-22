using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemmorageDebuff : MonoBehaviour
{
    public static IEnumerator Hemmorage_Debuff(EnemyStats enemyStats, float damage, EnemyDebuffHandler debuffHandler, int timeUntilDamage)
    {
        debuffHandler.hemmorageStacks++;
        while (timeUntilDamage > 0)
        {
            timeUntilDamage--;
            yield return new WaitForSeconds(1f);
        }
        debuffHandler.hemmorageStacks--;
        enemyStats.LiterallyJustTakeDamage((int)damage);
        yield break;
    }
}
