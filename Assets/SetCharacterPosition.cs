using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterPosition : MonoBehaviour
{
    public bool playerIsAtPos = false;
    // Start is called before the first frame update
    void Start()
    {
        //SetCharPos();
        Invoke("SetCharPos", 1f);
    }
    private void Update()
    {
        SetCharPos();
    }
    void SetCharPos()
    {
        if (playerIsAtPos) return;
        var player = FindObjectOfType<PlayerStats>();
        if (player.transform.position == gameObject.transform.position) playerIsAtPos = true;
        player.transform.position = gameObject.transform.position;
        GameObject.FindGameObjectWithTag("BossVictoryScreen").GetComponent<CanvasGroup>().alpha = 0;
        var loaders = FindObjectsOfType<LoadAfterTime>();
        foreach (LoadAfterTime loader in loaders) loader.ResetLoading();
        print("Set char pos");
    }
}
