using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public int R1StaminaCost;
    public int R2StaminaCost;
    public int R1Damage;
    public int R2Damage;
    public AnimatorOverrideController animations;
    public int unlockCost;
    public int R1PoiseDamage;
    public int R2PoiseDamage;
}
