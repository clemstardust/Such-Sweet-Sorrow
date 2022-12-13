using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AttackHitboxObject : MonoBehaviour
{
    public Collider dmgCollider;
    public WeaponItem weaponItem;

    
    // Start is called before the first frame update
    void Start()
    {
        dmgCollider = GetComponent<Collider>();
        
        dmgCollider.enabled = false;
    }
}
