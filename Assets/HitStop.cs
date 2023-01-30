using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyDamageHitbox")
        {
            playerAnimator.speed = 0.01f;
            Invoke("ResetAnimSpeed", 0.1f);
        }
    }

    public void ResetAnimSpeed()
    {
        playerAnimator.speed = 1f;
    }

}
