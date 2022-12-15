using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using Random = System.Random;

public class PlayerStats : MonoBehaviour
{

    PlayerActionHandler playerActionHandler;
    public float currentStamina = 50;
    public int maxStamina = 50;

    public float currentHealth = 100;
    public int maxHealth = 100;

    public int maxSoul = 0;
    public float currentSoul = 0;

    public float currentXP = 0;
    public float currentLevel = 0;

    public float staminaRegenRate = 10f;
    public float regenTimer = 0f;
    public float soulRegenRate = 500f;
    public float tempSoul;

    public int totalSouls = 0;

    public bool regenerateSoul = false;
    public bool degenerateSoul = false;

    public bool degenerateHealth;
    public int extraLives;

    public bool helathLowAddDamage = false;
    public bool staminaToDamage = false;
    public bool useMoreStaminaToDamage = false;
    public bool thorns = false;
    public bool extraHealthOnHit = false;
    public bool damageBuffOnDodge = false;
    public bool healOnDodge = false;

    public bool applyDotDebuff = false;
    
    public float staminaToDamageMuliplier = 1;

    public static bool isDead;
    public WeaponItem swordPH;
    public GameObject deathScreen;

    EnemyManager enemyManager;
    Animator animator;
    UIManager uIManager;
    PlayerEquipment playerEquipment;
    PlayerUpgradeHandler playerUpgradeHandler;

    private bool siphon = false;
    public bool soulShield = false;
    public float soulMuliplier = 1;

    public GameObject[] blood;

    // Start is called before the first frame update
    void Start()
    {
        playerActionHandler = gameObject.GetComponent<PlayerActionHandler>();
        animator = gameObject.GetComponent<Animator>();
        playerEquipment = gameObject.GetComponent<PlayerEquipment>();
        uIManager = gameObject.GetComponent<UIManager>();
        playerUpgradeHandler = gameObject.GetComponent<PlayerUpgradeHandler>();
        playerEquipment.currentWeapon = swordPH;
    }

    void FixedUpdate()
    {
        if (playerActionHandler.transformed && currentHealth <= 0)
        {
            playerActionHandler.Transition();
            currentHealth = maxHealth;
            Heal(0);
        }
        else if (currentHealth <= 0 && extraLives <= 0)
        {
            if (!animator.GetBool("Dead"))
            {
                UnlockedItemManager unlockedItemManager = FindObjectOfType<UnlockedItemManager>();
                unlockedItemManager.totalSouls += totalSouls;
                FindObjectOfType<GameSaveHandler>().SaveGame(unlockedItemManager);
                GameObject.FindGameObjectWithTag("RequiredLevelComponents").GetComponent<LoadAfterTime>().LoadSceneAfterDelay(5, "CharacerCreation");
                //isDead = true;
                animator.SetBool("Dead", true);
                //animator.SetBool("isInteracting", true);
                //this.enabled = false;
            }
        } 
        else if (currentHealth <= 0 && extraLives > 0)
        {
            extraLives--;
            currentHealth = maxHealth;
            uIManager.UpdateStatsUI(currentHealth, maxHealth);
            Heal(0);
        }
        else
        {
            
            if (degenerateHealth && extraLives > 0)
            {
                currentHealth -= ((currentHealth / maxHealth) + 1) * 0.015f;
                uIManager.UpdateStatsUI(currentHealth, maxHealth);
            }
            maxSoul = ((maxHealth - (int)currentHealth) + (int) playerUpgradeHandler.maxSoulUpLevel);
            if (playerActionHandler.transformed)
            {
                maxSoul = 0;
            }
            if (siphon && currentSoul < maxSoul)
            {
                currentSoul += 2 * Time.deltaTime;
            }
            RegnerateStamina();
            uIManager.UpdateStatsUI(currentStamina, currentHealth, currentSoul, maxHealth, maxStamina, playerEquipment);
        }
        
        RegenerateSoul();
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeHit(other);
    }

    private double CalculateTougherTimes(int numUpgrade)
    {
        if (numUpgrade > 0)
        {
            double RAND_CHANCE = 0.07f;
            print(1 - (1 / ((RAND_CHANCE * numUpgrade) + 1)));
            return 1 - (1 / ((RAND_CHANCE * numUpgrade) + 1));
        }
        else
        {
            return -1;
        }

    }

    public void TakeHit(Collider other)
    {
        
        if (other.CompareTag("EnemyAttackHitbox"))
        {
            double chanceToBlock = CalculateTougherTimes(playerUpgradeHandler.numTougherTimes);
            Random rand = new Random();
            double randomNumber = rand.NextDouble();
            print("Chance to block: " + chanceToBlock + " | random number: " + randomNumber);
            if (randomNumber <= chanceToBlock)
            {
                PlayerActionHandler.isInvul = true;
                print("Dodged attack automatically");

            }
            enemyManager = other.gameObject.GetComponentInParent<EnemyManager>();
            float damage = 0;
            switch (EnemyAI.currentAttack)
            {
                case EnemyAI.CurrentAttack.R1:
                    damage = enemyManager.equippedWeapon.R1Damage;
                    break;
                case EnemyAI.CurrentAttack.R2:
                    damage = enemyManager.equippedWeapon.R2Damage;
                    break;
            }
            if (!(gameObject.GetComponent<Animator>().GetBool("Roll")) && !PlayerActionHandler.isInvul)
            {
                PlayerActionHandler.isInvul = true;
                if (soulShield)
                {
                    if (currentSoul >= damage)
                    {
                        currentSoul -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= currentSoul;
                        currentSoul = 0;
                    }
                }
                FindObjectOfType<AudioManager>().PlayerHurtSound();
                currentHealth -= damage;
                if (thorns)
                {
                    other.GetComponentInParent<EnemyStats>().TakeHit((int) (damage * playerUpgradeHandler.thornsDamageMultiplier));
                }

                Instantiate(blood[UnityEngine.Random.Range(0, blood.Length - 1)], transform.position, Quaternion.identity);

                FindObjectOfType<HealthBarShrink>().Damage(currentHealth, maxHealth);
                animator.SetBool("ow", true);
                animator.SetBool("isInteracting", true);
            }
            else
            {
                PlayerActionHandler.isInvul = false;
                if (damageBuffOnDodge)
                {
                    print("Buffed damage");
                    playerActionHandler.damageAfterDodgeIsActive = true;
                    playerActionHandler.damageAfterDodgeCountdown = 3;
                }
                if (healOnDodge)
                {
                    print("healed on dodge");
                    //Heal((int)playerUpgradeHandler.dodgeToHealFlatAmount);
                    currentSoul += playerUpgradeHandler.dodgeToHealFlatAmount;
                }
            }
            uIManager.UpdateStatsUI(currentStamina, currentHealth, currentSoul, maxHealth, maxStamina, playerEquipment);
        }
    }
    private void RegnerateStamina()
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            regenTimer = 0;
        }

        if (!playerActionHandler.staminaRegenDelay)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer > 1f && currentStamina < maxStamina)
            {
                
                currentStamina += (staminaRegenRate * Time.deltaTime);
                FindObjectOfType<StaminaBarShrink>().Regenerate(currentStamina, maxStamina);
                if (currentStamina > maxStamina)
                {
                    currentStamina = maxStamina;
                
                }
            }
        }
    }
    public void RemoveStamina(float amount)
    {
        currentStamina -= amount;
        FindObjectOfType<StaminaBarShrink>().Degenenerate(currentStamina, maxStamina);
    }

    public void RegenerateSoul()
    {
        currentSoul += tempSoul * soulMuliplier;
        if (currentSoul > maxSoul){
            currentSoul = maxSoul;
        }
        tempSoul = 0;
    }

    public void EnemyKilled(int enemyTier)
    {
        print("enemy killed");
        if (enemyTier < 1000)
            currentXP += enemyTier;
        totalSouls += enemyTier;
        //print("total souls: " + totalSouls);
        maxSoul = maxHealth - (int) currentHealth;
        if (currentSoul < maxSoul)
        {
            tempSoul += enemyTier;
            regenerateSoul = true;
        }
        else
        {
            currentSoul = maxSoul;
        }
        if (enemyTier >= 2000)
        {
            UnlockedItemManager unlockedItemManager = FindObjectOfType<UnlockedItemManager>();
            unlockedItemManager.totalSouls += totalSouls;
            FindObjectOfType<GameSaveHandler>().SaveGame(unlockedItemManager);
            StartCoroutine(FadeAudioSource.StartFade(GameObject.FindGameObjectWithTag("MusicPlayerBoss").GetComponent<AudioSource>(), 3, 0));
            StartCoroutine(FadeAudioSource.StartFade(GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>(), 3, 1));
            //GameObject.FindGameObjectWithTag("MusicPlayerBoss").GetComponent<AudioSource>().Pause();
            //GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>().Play();
            GetComponent<LoadAfterTime>().LoadSceneAfterDelay(5, "CharacerCreation");
        }
    }

    public void UpgradeHealth(int num)
    {
        maxHealth += num;
        FindObjectOfType<HealthBarShrink>().Heal(currentHealth, maxHealth);
    }
    public void Heal(int num)
    {
        currentHealth += num;
        FindObjectOfType<HealthBarShrink>().Heal(currentHealth, maxHealth);
    }

    public void PhilospherRing()
    {
        degenerateHealth = true;
        extraLives++;
        //print("hi");
    }

    public void ExtraSoulRelic()
    {
        soulMuliplier += 0.2f;
    }

    public void SoulShield()
    {
        soulShield = true;
    }
    public void ActivateSiphon()
    {
        siphon = true;
    }

    public void ActivateDOTDebuff()
    {
        applyDotDebuff = true;
    }
}
