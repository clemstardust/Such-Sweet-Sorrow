using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int globalDarkness = 1; //between 0-3, sets the corrption value of a level.
    public static bool spawnedEndRoom;
    public static bool loadFromSave = false;

    public static int selectedStartingWeapon;
    public static int selectedStartingRelic;
    public static int selectedStartingSpell;

    public static WeaponItem selectedStartingWeaponData;
    public static RelicItem selectedStartingRelicData;
    public static RelicItem selectedStartingSpellData;

}
