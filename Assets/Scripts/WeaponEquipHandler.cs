using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponEquipHandler : MonoBehaviour
{
    public GameObject[] UpdateWeaponItems()
    {
        return GameObject.FindGameObjectsWithTag("Weapon");
    }
}
