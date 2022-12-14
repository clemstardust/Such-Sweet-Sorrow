using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TwoHandedAttackHitboxObject : MonoBehaviour
{
    public Collider dmgCollider;
    public Collider otherCollider;
    public WeaponItem weaponItem;

    
    // Start is called before the first frame update
    void Start()
    {
        dmgCollider = GetComponent<Collider>();
        dmgCollider.enabled = false;

        otherCollider = GetComponent<Collider>();
        otherCollider.enabled = false;
    }
}
