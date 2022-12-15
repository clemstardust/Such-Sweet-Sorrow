using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDebuffHandler : MonoBehaviour
{
    

    private EnemyStats enemyStats;
    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
