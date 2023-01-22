using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnTarget : MonoBehaviour
{
    public bool isDead = false;
    public Outline indicator;
    private void Start()
    {
        indicator = GetComponentInParent<EnemyAI>().gameObject.GetComponentInChildren<Outline>();
        indicator.enabled = false;
    }

}
