using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    EnemyManager enemyManager;
    public PlayerStats currentTarget;
    public float distanceToTarget;

    public GameObject raycastOrigin;
    private EnemyStats enemyStats;
    private Animator animator;
    private NavMeshAgent agent;
    private AttackHitboxObject attackHitbox;
    public bool rotated;
    private Quaternion rotation;

    public enum CurrentAttack
    {
        R1,
        R2

    };
    public static CurrentAttack currentAttack;

    Vector3 spawn;
    [SerializeField] LayerMask detectionLayer;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
        attackHitbox = GetComponentInChildren<AttackHitboxObject>();

        spawn = transform.position;
        rotation = transform.rotation;

        agent.stoppingDistance = enemyStats.attackRange;
    }

    private void Update()
    {
        if (currentTarget != null)
            distanceToTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        layerMask = ~layerMask;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

    }

    private PlayerStats playerStats;
    public void HandleDetection()
    {   
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        playerStats = FindObjectOfType<PlayerStats>();
        Vector3 targetDirection = playerStats.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        // Does the ray intersect any objects excluding the player layer
        Vector3 dir = playerStats.gameObject.transform.position - transform.position;
        dir.y = 0;
        if (Physics.Raycast(raycastOrigin.transform.position, dir, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
            if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerDamageCollider"))
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                currentTarget = playerStats;
            }
        }
        /*
        if (viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle
        || enemyManager.autoDetectDist >= Vector3.Distance(playerStats.transform.position, transform.position))
        {

            // Does the ray intersect any objects excluding the player layer
            Vector3 dir = playerStats.gameObject.transform.position - transform.position;
            dir.y = 0;
            if (Physics.Raycast(raycastOrigin.transform.position, dir, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerDamageCollider"))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                    currentTarget = playerStats;
                }
            }

        }*/
        /*
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        foreach (Collider col in colliders)
        {
            playerStats = col.transform.GetComponentInParent<PlayerStats>();

            if (playerStats != null)
            {
                Vector3 targetDirection = playerStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                if(viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle 
                || enemyManager.autoDetectDist >= Vector3.Distance(playerStats.transform.position, transform.position)) 
                {

                    // Does the ray intersect any objects excluding the player layer
                    Vector3 dir = playerStats.gameObject.transform.position - transform.position;
                    dir.y = 0;
                    if (Physics.Raycast(raycastOrigin.transform.position, dir, out RaycastHit hit, Mathf.Infinity, layerMask))
                    {
                        Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                        if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerDamageCollider"))
                        {
                            Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                            currentTarget = playerStats;
                        }
                    }
                    
                }
            }
        }*/
    }

    internal void ReturnToSpawn()
    {
        agent.SetDestination(spawn);
        animator.SetFloat("Speed", 2);
    }

    public void HandleMoveToTarget()
    {
        /*if (!rotated)
        {
            agent.SetDestination(currentTarget.transform.position);
            Vector3 dir = agent.steeringTarget - agent.transform.position;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                //agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rot, agent.angularSpeed * Time.deltaTime);
            }
        //}*/
        FaceTarget();

        if (Vector3.Distance(currentTarget.transform.position, transform.position) <= enemyStats.attackRange)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
            //agent.Stop();
        }
        else
        {
            agent.SetDestination(currentTarget.transform.position);
            animator.SetFloat("Speed", 2);
            //agent.Resume();
            agent.isStopped = false;
        }
    }

    //I stole this from https://answers.unity.com/questions/1410325/question-about-navmesh-agent-and-rotation.html
    public void FaceTarget()
    {
        //get difference of the rotation of the player and gameObjects position
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;

        //set lookRotation to the x and y of the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void Attack()
    {
        FaceTarget();
        //rotation = Quaternion.LookRotation(currentTarget.gameObject.transform.position, Vector3.up);
        //agent.transform.rotation = rotation;        
        //rotation = transform.rotation;
        int randAttackChance = Mathf.RoundToInt(UnityEngine.Random.Range(0, 4));
        if (enemyStats.currentStamina >= enemyManager.equippedWeapon.R2StaminaCost && randAttackChance == 1)
        {
            currentAttack = CurrentAttack.R2;
            animator.SetBool("Attack2", true);
        }
        else if (enemyStats.currentStamina >= enemyManager.equippedWeapon.R1StaminaCost && randAttackChance == 2)
        {
            currentAttack = CurrentAttack.R1;
            animator.SetBool("Attack", true);
            animator.SetInteger("Combo", 2);
        }
        else if (enemyStats.currentStamina <= 0)
        {
            return;
        }
        else
        {
            animator.SetBool("Attack", true);
            animator.SetInteger("Combo", 0);
            currentAttack = CurrentAttack.R1;
        }
        //transform.rotation = rotation;
        //enemyManager.enemyMode = EnemyManager.Mode.chase;
    }

    public void Idle()
    {
        animator.SetFloat("Speed", 0);
    }

    public void DisableCombo()
    {
        animator.SetBool("Attack", true);
        DisableCollider();
        rotated = false;
    }

    public void EnableCollider()
    {
        int staminaCost = 0;
        attackHitbox.dmgCollider.enabled = true;
        switch (currentAttack)
        {
            case CurrentAttack.R1:
                staminaCost = enemyManager.equippedWeapon.R1StaminaCost;
                break;
            case CurrentAttack.R2:
                staminaCost = enemyManager.equippedWeapon.R2StaminaCost;
                break;
        }
        enemyStats.LoseStamina(staminaCost);
    }
    private void DisableCollider()
    {
        attackHitbox.dmgCollider.enabled = false;
        rotated = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!animator.GetBool("Dead"))
        {
            enemyStats.TakeHit(other);
        }
            
    }
    public void Dead()
    {
        animator.SetBool("Dead", true);
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<EnemyManager>().enabled = false;
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponentInChildren<AttackHitboxObject>().gameObject.SetActive(false);
        //GetComponent<Rigidbody>().isKinematic = true;
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (CapsuleCollider col in colliders)
        {
            col.enabled = false;
        }
        playerStats.EnemyKilled(enemyStats.enemySoulLevel);
    }
}
