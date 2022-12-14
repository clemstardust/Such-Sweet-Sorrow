using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    EnemyAI enemyAI;
    [Header("AI Settings")]
    public float detectionRadius = 20;
    //the higher, qand lower, respectivly, these angle are the greater detection FOV
    public float maxDetectionAngle = 50;
    public float minDetectionAngle = -50;
    public float autoDetectDist = 2;

    public WeaponItem equippedWeapon;

    private EnemyStats stats;

    [SerializeField]
    public enum Mode
    {
        idle,
        chase,
        attack,
        dead,
        returning

    };
    public Mode enemyMode;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        stats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMode != Mode.dead)
        {
            if (enemyAI.currentTarget == null)
            {
                enemyMode = Mode.idle;
            }
            else if (enemyAI.distanceToTarget <= stats.attackRange)
            {
                enemyMode = Mode.attack;
            }
            else
            {
                enemyMode = Mode.chase;
            }
        }
        else
        {
            var colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            var moreColliders = GetComponents<Collider>();
            foreach (Collider col in moreColliders)
            {
                col.enabled = false;
            }
        }
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        switch (enemyMode)
        {
            case Mode.dead:
                enemyAI.Dead();
                break;
            case Mode.attack:
                enemyAI.Attack();
                break;
            case Mode.chase:
                enemyAI.HandleMoveToTarget();
                break;
            case Mode.returning:
                enemyAI.ReturnToSpawn();
                break;
            case Mode.idle:
                enemyAI.HandleDetection();
                enemyAI.Idle();
                break;
            default:
                enemyAI.HandleDetection();
                break;
        }
    }

   

}