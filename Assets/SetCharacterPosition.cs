using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterPosition : MonoBehaviour
{
    bool playerIsAtPos;
    // Start is called before the first frame update
    void Start()
    {
        //SetCharPos();
        Invoke("SetCharPos", 1f);
    }

    void SetCharPos()
    {
        var player = FindObjectOfType<PlayerStats>();
        player.transform.position = gameObject.transform.position;
        GameObject.FindGameObjectWithTag("BossVictoryScreen").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.FindGameObjectWithTag("BossVictoryScreen").gameObject.SetActive(false);
        var loaders = FindObjectsOfType<LoadAfterTime>();
        foreach (LoadAfterTime loader in loaders) loader.ResetLoading();
        print("Set char pos");
    }
}
