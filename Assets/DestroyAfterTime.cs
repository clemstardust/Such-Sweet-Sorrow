using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyAfter = 1;
    // Update is called once per frame
    void Update()
    {
        destroyAfter -= Time.deltaTime;
        if (destroyAfter <= 0) Destroy(gameObject);
    }
}
