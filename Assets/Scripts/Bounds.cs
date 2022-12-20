using System.Collections.Generic;
using UnityEngine;


public class Bounds : MonoBehaviour
{
    public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject.GetComponentInParent<TileProperties>().gameObject);
        print("Destroyed " + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Destroy(collision.gameObject.GetComponentInParent<TileProperties>().gameObject);
        print("Destroyed " + collision.gameObject.name);
    }


}

