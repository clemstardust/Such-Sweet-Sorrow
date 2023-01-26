using System.Collections.Generic;
using UnityEngine;


public class Bounds : MonoBehaviour
{
    public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();
    public Collider col;
    private void Awake()
    {
        col = GetComponentInChildren<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Destroyed " + collision.gameObject.name);

        Destroy(collision.gameObject.GetComponentInParent<TileProperties>().gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        print("Destroyed " + collision.gameObject.name);

        Destroy(collision.gameObject.GetComponentInParent<TileProperties>().gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Destroyed " + other.gameObject.name);

        Destroy(other.gameObject.GetComponentInParent<TileProperties>().gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        print("Destroyed " + other.gameObject.name);

        Destroy(other.gameObject.GetComponentInParent<TileProperties>().gameObject);
    }


}

