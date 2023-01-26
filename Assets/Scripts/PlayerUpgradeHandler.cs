using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeHandler : MonoBehaviour
{
    public float damageMultiplier = 1;
    public float healthMultiplier = 1;
    public float staminaMultiplier = 1;
    public float spellDamageMuliplier = 1;
    
    public float healthLowDamageMultiplier = 2f;
    public float maxSoulUpLevel = 1;
    public float extraStaminaToDamageMultiplier = 1.5f;
    public float thornsDamageMultiplier = 1;
    public float flatExtraHealthOnHit = 10;
    public float dodgeDamageMultiplier = 1.5f;
    public float dodgeToHealFlatAmount = 10;
    public int numTougherTimes = 0;
    public int numTrans = 0;

    //status effect chance
    public float poisonChance = 0;
    public float iceChance = 0;
    public float bleedChance = 0;
    public float rotChance = 0;
    public float hemmorageChance = 0;
    public float stunChance = 0;

    //unused
    public float spellCostDownPercent = 1;
    public float spellDurationMultiplier = 1;

    //xp
    private float xpToNextLevel;

    public GameObject xpBar;
    //ui
    public GameObject upgradeMenu;
    //for relic
    public bool doubleUpgrades = false;
    //references
    private PlayerStats playerStats;
    private PlayerActionHandler playerActionHandler;
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerActionHandler = GetComponent<PlayerActionHandler>();
        upgradeMenu.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        xpToNextLevel = 100 + (Mathf.Pow(playerStats.currentLevel, 2) * 10);

        xpBar.GetComponent<Slider>().maxValue = xpToNextLevel;
        xpBar.GetComponent<Slider>().value = playerStats.currentXP;
        if (playerStats.currentHealth < 0)
        {
            Time.timeScale = 1;
        }
        //print(xpToNextLevel);
        if (playerStats.currentXP >= xpToNextLevel && upgradeMenu.activeSelf == false && playerStats.currentHealth > 0)
        {
            upgradeMenu.SetActive(true);
            upgradeMenu.GetComponent<UpgradeMenu>().ReloadMenu();
        }
    }

    public void DoubleUpgrades()
    {
        doubleUpgrades = true;
    }

    private void ResolveUpgrade()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerStats.currentXP -= xpToNextLevel;
        playerStats.currentLevel++;
        upgradeMenu.SetActive(false);
    }

    public void UpgradeDamage()
    {
        
        print("Damage upgraded!");
        damageMultiplier += 0.1f;
        if (doubleUpgrades)
        {
            damageMultiplier += 0.3f;
        }

        ResolveUpgrade();
    }

    public void UpgradeHealth ()
    {
        print("Health upgraded!");
        healthMultiplier += 0.5f;
        if (doubleUpgrades)
        {
            healthMultiplier += 0.5f;
        }
        playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);
        FindObjectOfType<HealthBarShrink>().Heal(playerStats.currentHealth, playerStats.maxHealth);
        ResolveUpgrade();
    }
    public void UpgradeStamina()
    {
        print("Stamina upgraded!");
        staminaMultiplier += 0.5f;
        if (doubleUpgrades)
        {
            staminaMultiplier += 0.5f;
        }
        playerStats.maxStamina = (int)(playerStats.maxStamina * staminaMultiplier);
        FindObjectOfType<StaminaBarShrink>().Regenerate(playerStats.currentStamina, playerStats.maxStamina);
        ResolveUpgrade();
    }

    public void StaminaToDamage()
    {        
        if (doubleUpgrades)
        {
            playerStats.staminaToDamageMuliplier += 0.2f;
        }
        if (playerStats.staminaToDamage)
        {
            playerStats.staminaToDamageMuliplier += 0.2f;
        }
        playerStats.staminaToDamage = true;
        print("Stamina to damage upgraded!");
        ResolveUpgrade();
    }
    public void SpellDamageUp()
    {
        if (doubleUpgrades)
        {
            spellDamageMuliplier += 1f;
        }
        spellDamageMuliplier += 1f;
        print("Apell damage upgraded!");
        ResolveUpgrade();
    }
    public void SpellCostDown()
    {
        if (doubleUpgrades)
        {
            spellCostDownPercent -= 0.1f;
        }
        spellCostDownPercent -= 0.1f;
        print("Spell cost down upgraded");
        ResolveUpgrade();
    }
    public void SpellDurationUp()
    {
        if (doubleUpgrades)
        {
            spellDurationMultiplier += 0.33f;
        }
        spellDurationMultiplier += 0.33f;
        print("Spell duration upgraded!");
        ResolveUpgrade();
    }
    public void HealthLowDamageUpgrade()
    {
        if (doubleUpgrades)
        {
            healthLowDamageMultiplier += 1f;
        }
        if (playerStats.helathLowAddDamage)
        {
            healthLowDamageMultiplier += 1f;
        }
        playerStats.helathLowAddDamage = true;
        print("Spell duration upgraded!");
        ResolveUpgrade();
    }
    public void MaxSoulUp()
    {
        if (doubleUpgrades)
        {
            maxSoulUpLevel += 50f;
        }
        maxSoulUpLevel += 50f;
        print("max soul upgraded!");
        ResolveUpgrade();
    }
    public void ExtraDamageAtCostOfStamina()
    {
        if (doubleUpgrades)
        {
            extraStaminaToDamageMultiplier += 0.5f;
        }
        if (playerStats.useMoreStaminaToDamage)
        {
            extraStaminaToDamageMultiplier += 0.5f;
        }
        playerStats.useMoreStaminaToDamage = true;
        print("Extra damage at cost of stamina upgraded!");
        ResolveUpgrade();
    }
    public void Thorns()
    {
        if (doubleUpgrades)
        {
            thornsDamageMultiplier += 0.1f;
        }
        if (playerStats.useMoreStaminaToDamage)
        {
            thornsDamageMultiplier += 0.1f;
        }
        playerStats.thorns = true;
        print("thorns upgraded!");
        ResolveUpgrade();
    }

    public void ExtraHealthOnHit()
    {
        if (doubleUpgrades)
        {
            flatExtraHealthOnHit += 2f;
        }
        if (playerStats.extraHealthOnHit)
        {
            flatExtraHealthOnHit += 2f;
        }
        playerStats.extraHealthOnHit = true;
        print("extra health on hit upgraded!");
        ResolveUpgrade();
    }
    public void ExtraDamageOnDodge()
    {
        if (doubleUpgrades)
        {
            dodgeDamageMultiplier += 1f;
        }
        if (playerStats.damageBuffOnDodge)
        {
            dodgeDamageMultiplier += 1f;
        }
        playerStats.damageBuffOnDodge = true;
        print("damage on dodge upgraded!");
        ResolveUpgrade();
    }
    public void HealOnDodge()
    {
        if (doubleUpgrades)
        {
            dodgeToHealFlatAmount += 15f;
        }
        if (playerStats.healOnDodge)
        {
            dodgeToHealFlatAmount += 15f;
        }
        playerStats.healOnDodge = true;
        print("heal on dodge upgraded!");
        ResolveUpgrade();
    }
    public void TougherTimes()
    {
        if (doubleUpgrades)
        {
            numTougherTimes++;
        }
        numTougherTimes++;
        print("num tougher times upgraded");
        ResolveUpgrade();
    }

    public void TransPride()
    {
        if (doubleUpgrades)
        {
            numTrans++;
        }
        numTrans++;
        if (numTrans >= 2)
        {
            numTrans = 0;
            playerActionHandler.Transition();
        }
        print("num trans upgraded");
        ResolveUpgrade();
    }

    public void StunChanceUp()
    {
        if (doubleUpgrades)
        {
            stunChance += 0.2f;
        }
        stunChance += 0.2f;
        ResolveUpgrade();
    }

    public void HemmorageChanceUp()
    {
        if (doubleUpgrades)
        {
            hemmorageChance += 0.2f;
        }
        hemmorageChance += 0.2f;
        ResolveUpgrade();
    }

    public void RotChanceUp()
    {
        if (doubleUpgrades)
        {
            rotChance += 0.2f;
        }
        rotChance += 0.2f;
        ResolveUpgrade();
    }

    public void BleedChanceUp()
    {
        if (doubleUpgrades)
        {
            bleedChance += 0.2f;
        }
        bleedChance += 0.2f;
        ResolveUpgrade();
    }

    public void IceChanceUp()
    {
        if (doubleUpgrades)
        {
            iceChance += 0.2f;
        }
        iceChance += 0.2f;
        ResolveUpgrade();
    }

    public void PoisionChanceUp()
    {
        if (doubleUpgrades)
        {
            poisonChance += 0.2f;
        }
        poisonChance += 0.2f;
        ResolveUpgrade();
    }

}
