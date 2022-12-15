using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedItemManager : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<GameSaveHandler>().LoadGame();
    }
    public int totalSouls;
    public bool unlockedSword = true;
    public bool unlockedMace = true;
    public bool unlockedBigSword = false;
    public bool unlockedBigMace = false;
    public bool unlockedExtraLivesRing = false;
    public bool unlockedExtraSoulsRing = true;
    public bool unlockedSoulshield = false;
    public bool unlockedSoulfire = false;
    public bool unlockedSoulGenerator = false;
    public bool unlockedDoubleUpgrades = false;
    public bool unlockedDOTRelic = false;
    public bool unlockedExtraDamageOnUndamaged = false;
    public bool unlockedExtraXP = false;
    public bool unlockedSoulOnHit = false;
}
