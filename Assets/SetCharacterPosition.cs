using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerStats>().gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerStartPos").transform.position;
        GameObject.FindGameObjectWithTag("BossVictoryScreen").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.FindGameObjectWithTag("BossVictoryScreen").gameObject.SetActive(false);
        var loaders = FindObjectsOfType<LoadAfterTime>();
        foreach (LoadAfterTime loader in loaders) loader.ResetLoading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
