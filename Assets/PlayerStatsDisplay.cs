using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerUpgradeHandler playerUpgradeHandler;
    public PlayerActionHandler playerActionHandler;

    public TextMeshProUGUI health;
    public TextMeshProUGUI stamina;
    public TextMeshProUGUI XP;
    public TextMeshProUGUI maxDamageMultiplier;
    public TextMeshProUGUI currentSpell;
    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnEnable()
    {
        health.text = "Health: " + playerStats.currentHealth + " / " + playerStats.maxHealth;
        stamina.text = "Maximum Stamina: " + playerStats.maxStamina;
        XP.text = "Experience: " + playerStats.currentXP + " / " + (100 + (Mathf.Pow(playerStats.currentLevel, 2) * 10));
        float damageMuliplier = 0;
        damageMuliplier += playerUpgradeHandler.damageMultiplier;
        if (playerStats.helathLowAddDamage && (playerStats.currentHealth / playerStats.maxHealth) <= 0.25f)
        {
            damageMuliplier += playerUpgradeHandler.healthLowDamageMultiplier;
        }
        if (playerStats.useMoreStaminaToDamage)
        {
            damageMuliplier += playerUpgradeHandler.extraStaminaToDamageMultiplier;
        }
        if (playerStats.damageBuffOnDodge && playerActionHandler.damageAfterDodgeIsActive)
        {
            damageMuliplier += playerUpgradeHandler.dodgeDamageMultiplier;
        }
        if (playerStats.extraDamageOnUndamaged)
        {
            damageMuliplier += 4;
        }
        if (playerStats.attackingReducesHealth)
        {
            damageMuliplier += 2f;
        }
        if (playerStats.glass)
        {
            damageMuliplier += 2;
        }
        maxDamageMultiplier.text = "Max damage multiplier: " + damageMuliplier;
        currentSpell.text = "Current invocation: None";
        if (playerActionHandler.canCast)
        {
            switch (PlayerActionHandler.currentSpell)
            {
                case PlayerActionHandler.CurrentSpell.ChannelSoulToDamage:
                    currentSpell.text = "Current invocation: Essence Flux";
                    break;
                case PlayerActionHandler.CurrentSpell.Immolation:
                    currentSpell.text = "Current invocation: Immolation";
                    break;
                case PlayerActionHandler.CurrentSpell.Soulfire:
                    currentSpell.text = "Current invocation: Soulfire Weapon";
                    break;
                case PlayerActionHandler.CurrentSpell.SoulForm:
                    currentSpell.text = "Current invocation: Soulform";
                    break;
            }
        }        
    }
}
