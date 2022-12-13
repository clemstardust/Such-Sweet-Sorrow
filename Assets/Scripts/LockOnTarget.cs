using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField] GameObject lockIndicator;
    //[SerializeField] public bool locked;

    private void Start()
    {
        lockIndicator = transform.GetChild(0).gameObject;
        OnUnlock();
    }

    public void OnLock()
    {
        lockIndicator.SetActive(true);
    }
    public void OnUnlock()
    {
        lockIndicator.SetActive(false);
    }

}
