using System.Collections.Generic;
using UnityEngine;


public class Bounds : MonoBehaviour
{
    public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();
}

