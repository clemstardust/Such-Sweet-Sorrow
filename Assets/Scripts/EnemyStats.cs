using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    public Slider healthBar;
    public BetterEnemyHealthbar betterHealthBar;
    [Header("Stamina")]
    public float maxStamina = 50;
    public float currentStamina;
    public float staminaRegenRate = 0.5f;
    [Header("Attacks")]
    public float attackRange = 1;
    [Header("Poise")]
    public int maxPoise = 100;
    public float currentPoise;
    public float poiseRegenDelay = 3;
    public float poiseRegenRate = 25;
    private float poiseRegenTimer;
    [Header("Soul and XP Gain")]
    public int enemySoulLevel = 1;
    [Header("Hit effects")]
    public GameObject hitParticles;
    public GameObject[] blood;
    public GameObject bloodParticles;
    public GameObject bloodParticles1;
    public AudioClip[] hitSoundEffects;

    private float regenTimer = 0;
    private PlayerEquipment playerEquipment;
    private PlayerStats playerStats;
    private PlayerUpgradeHandler playerUpgradeHandler;
    private EnemyAI enemyAI;
    private EnemyDebuffHandler debuffHandler;
    private EnemyManager manager;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        playerEquipment = FindObjectOfType<PlayerEquipment>();
        debuffHandler = GetComponent<EnemyDebuffHandler>();
        manager = GetComponent<EnemyManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerUpgradeHandler = FindObjectOfType<PlayerUpgradeHandler>();
        healthBar.maxValue = maxHealth;
        betterHealthBar.SetHealth(betterHealthBar.GetHealthNormalized(currentHealth,maxHealth));
        maxStamina = 25;
        enemyAI = GetComponent<EnemyAI>();
        currentPoise = maxPoise;
    }

    private void FixedUpdate()
    {
        RegnerateStamina();
        healthBar.value = currentHealth;
        if (currentHealth == maxHealth && enemySoulLevel < 2000)
        {
            betterHealthBar.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if (manager.enemyMode != EnemyManager.Mode.dead)
        {
            betterHealthBar.gameObject.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            betterHealthBar.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void TakeHit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            var playerCam = FindObjectOfType<CameraMotionHandler>();
            playerCam.TurnOffIndicator();
            playerCam.closest = GetComponentInChildren<CameraTarget>();
            playerCam.lockedOn = true;
            GetComponent<AudioSource>().volume = 0.25f;
            GetComponent<AudioSource>().PlayOneShot(hitSoundEffects[Random.Range(0, hitSoundEffects.Length)]);
            Animator animator = gameObject.GetComponent<Animator>();
            Instantiate(blood[(int)Random.Range(0, blood.Length)], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));

            var a = Instantiate(bloodParticles, transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            a.transform.parent = transform;
            a.transform.localPosition = new Vector3(a.transform.localPosition.x, a.transform.localPosition.y + 0.25f + Random.value, a.transform.localPosition.z);

            var b = Instantiate(bloodParticles1, transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            b.transform.parent = transform;
            b.transform.localPosition = new Vector3(b.transform.localPosition.x, b.transform.localPosition.y + 0.25f + Random.value, b.transform.localPosition.z);

            float damage = playerEquipment.currentWeapon.R1Damage;
            if (other.gameObject.GetComponentInParent<Animator>().GetBool("AttackR2"))
            {
                damage = playerEquipment.currentWeapon.R2Damage;
            }
            var playerStats = other.GetComponentInParent<PlayerStats>();
            var playerUpgradeHandler = other.gameObject.GetComponentInParent<PlayerUpgradeHandler>();
            var playerActionHandler = other.GetComponentInParent<PlayerActionHandler>();
            damage += (playerActionHandler.extraDamageFromSoul * playerUpgradeHandler.spellDamageMuliplier);
            damage += ((maxHealth * 0.001f) * debuffHandler.bleedStacks);
            if (playerActionHandler.extraDamageFromSoul > 0)
                playerActionHandler.extraDamageFromSoul = 0;
            if (playerStats.staminaToDamage)
            {
                damage += (playerStats.maxStamina - playerStats.currentStamina) * playerStats.staminaToDamageMuliplier;
            }
            damage = ((damage * (playerUpgradeHandler.damageMultiplier) * (other.gameObject.GetComponentInParent<PlayerActionHandler>().attackMultiplier)));
            if (playerStats.helathLowAddDamage && (playerStats.currentHealth / playerStats.maxHealth) <= 0.25f)
            {
                damage *= playerUpgradeHandler.healthLowDamageMultiplier;
            }
            if (playerStats.useMoreStaminaToDamage)
            {
                damage *= playerUpgradeHandler.extraStaminaToDamageMultiplier;
            }
            if (playerStats.damageBuffOnDodge && playerActionHandler.damageAfterDodgeIsActive)
            {
                damage *= playerUpgradeHandler.dodgeDamageMultiplier;
            }
            if (playerStats.applyDotDebuff)
            {
                StartCoroutine(DOTDebuff.DOT_Debuff(this, damage, 3, debuffHandler));
            }
            if (playerStats.extraDamageOnUndamaged && maxHealth == currentHealth)
            {
                damage *= 4;
            }
            if (playerStats.attackingReducesHealth)
            {
                damage *= 2f;
            }
            if (playerStats.glass)
            {
                damage *= 2;
            }
            if (Random.Range(0,100) <= debuffHandler.rotStacks * 10)
            {
                damage *= 2;
            }
            CalculateStatusesApplied((int)damage);
            damage *= 1 + (0.1f * (debuffHandler.stunStacks + debuffHandler.rotStacks + debuffHandler.poisonStacks + debuffHandler.iceStacks + debuffHandler.hemmorageStacks + debuffHandler.darkStacks + debuffHandler.bleedStacks));
            print("Damage: " + damage /*+ " | Extra stamina damage: " + ((playerStats.maxStamina - playerStats.currentStamina) * playerStats.staminaToDamageMuliplier) + " | damage mulitplier from upgrades: " + other.gameObject.GetComponentInParent<PlayerUpgradeHandler>().damageMultiplier + " | Attack muliplier from spells: " + other.gameObject.GetComponentInParent<PlayerActionHandler>().attackMultiplier*/ );
            currentHealth -= damage;
            betterHealthBar.Damage(currentHealth, maxHealth, (int) damage);
            gameObject.GetComponent<EnemyAI>().rotated = false;
            if (playerStats.extraHealthOnHit)
            {
                playerStats.UpgradePlayerHealth((int) (damage * 0.01f * playerUpgradeHandler.flatExtraHealthOnHit));
            }
            if (playerStats.soulOnHit)
            {
                playerStats.Heal(15);
            }
            if (currentHealth <= 0)
            {
                GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
                healthBar.gameObject.SetActive(false);
                GetComponentInChildren<StatusBar>().gameObject.SetActive(false);
                GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
                var particles = GameObject.FindGameObjectsWithTag("StatusParticle");
                foreach (GameObject obj in particles)
                {
                    Destroy(obj);
                }
            }
            //Poise
            float poiseDamage = playerEquipment.currentWeapon.R1Damage;
            if (other.gameObject.GetComponentInParent<Animator>().GetBool("AttackR2"))
            {
                poiseDamage = playerEquipment.currentWeapon.R2Damage;
            }
            currentPoise -= poiseDamage * (1 + (1.25f * debuffHandler.stunStacks));
            if (currentPoise <= 0)
            {
                animator.SetBool("Hit", true);
                currentPoise = maxPoise;
                enemyAI.LiterallyJustDiasbleTheDamnCOllider();
            }            
        }
    }

    public void CalculateStatusesApplied(int damage)
    {
        PlayerUpgradeHandler upgrades = FindObjectOfType<PlayerUpgradeHandler>();
        StatusBar statusBar = GetComponentInChildren<StatusBar>();
        if (statusBar == null)
            statusBar = GameObject.Find("StatusBarBoss").GetComponent<StatusBar>();
        int newPoisonStacks = (int)(upgrades.poisonChance + Random.Range(0, 0.99f));
        int newIceStacks = (int)(upgrades.iceChance + Random.Range(0, 0.99f));
        int newBleedStacks = (int)(upgrades.bleedChance + Random.Range(0, 0.99f));
        int newRotStacks = (int)(upgrades.rotChance + Random.Range(0, 0.99f));
        int newHemStacks = (int)(upgrades.hemmorageChance + Random.Range(0, 0.99f));
        int newStunStacks = (int)(upgrades.stunChance + Random.Range(0, 0.99f));
        if (newPoisonStacks > 0 && debuffHandler.poisonStacks == 0) { 
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Poison);
            var newparticle = Instantiate(statusBar.statusEffectParticles[0], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }
        if (newIceStacks > 0 && debuffHandler.iceStacks == 0) { 
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Ice);
            var newparticle = Instantiate(statusBar.statusEffectParticles[1], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }
        if (newBleedStacks > 0 && debuffHandler.bleedStacks == 0) { 
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Bleed);
            var newparticle = Instantiate(statusBar.statusEffectParticles[2], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }
        if (newRotStacks > 0 && debuffHandler.rotStacks == 0) { 
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Rot);
            var newparticle = Instantiate(statusBar.statusEffectParticles[3], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }
        if (newHemStacks > 0 && debuffHandler.hemmorageStacks == 0) { 
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Hemmorage);
            var newparticle = Instantiate(statusBar.statusEffectParticles[4], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }
        if (newStunStacks > 0 && debuffHandler.stunStacks == 0) {
            statusBar.AddStatus(StatusEnum.DisplayedDebuff.Stun);
            var newparticle = Instantiate(statusBar.statusEffectParticles[5], transform.position, new Quaternion(0, Random.rotation.y, 0, 0));
            newparticle.transform.parent = transform;
        }

        for (int i = 0; i < newHemStacks; i++)
        {
            StartCoroutine(HemmorageDebuff.Hemmorage_Debuff(this, damage, debuffHandler, 3));
        }
        for (int i = 0; i < newPoisonStacks; i++)
        {
            StartCoroutine(PoisonDebuff.Poison_Debuff(this, 2));
        }
        debuffHandler.poisonStacks += newPoisonStacks;
        debuffHandler.iceStacks += newIceStacks;
        debuffHandler.bleedStacks += newBleedStacks;
        debuffHandler.rotStacks += newRotStacks;
        debuffHandler.hemmorageStacks += newHemStacks;
        debuffHandler.stunStacks += newStunStacks;
    }

    public float countDown = 1;
    public void TakeImmolationHit()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
           
            GetComponent<AudioSource>().pitch = 0.8f;
            GetComponent<AudioSource>().Play();
            Animator animator = gameObject.GetComponent<Animator>();
            var playerUpgradeHandler = playerStats.gameObject.GetComponentInParent<PlayerUpgradeHandler>();

            float damage = 25 * playerUpgradeHandler.spellDamageMuliplier;
            Instantiate(blood[(int)Random.Range(0, blood.Length)], transform.position, Quaternion.identity);
            print("Damage from immolation: " + damage /*+ " | Extra stamina damage: " + ((playerStats.maxStamina - playerStats.currentStamina) * playerStats.staminaToDamageMuliplier) + " | damage mulitplier from upgrades: " + other.gameObject.GetComponentInParent<PlayerUpgradeHandler>().damageMultiplier + " | Attack muliplier from spells: " + other.gameObject.GetComponentInParent<PlayerActionHandler>().attackMultiplier*/ );
            currentHealth -= damage;
            betterHealthBar.Damage(currentHealth, maxHealth, (int) damage);
            animator.SetBool("Hit", true);
            gameObject.GetComponent<EnemyAI>().rotated = false;
            if (playerStats.extraHealthOnHit)
            {
                playerStats.UpgradePlayerHealth((int)playerUpgradeHandler.flatExtraHealthOnHit);
            }
            if (currentHealth <= 0)
            {
                healthBar.gameObject.SetActive(false);
                GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
            }
            countDown = 1;
            enemyAI.LiterallyJustDiasbleTheDamnCOllider();
        }
    }

    public void TakeHit(int damage)
    {
        GetComponent<AudioSource>().pitch = 0.8f;
        GetComponent<AudioSource>().Play();
        Animator animator = gameObject.GetComponent<Animator>();
        Instantiate(blood[(int)Random.Range(0, blood.Length)], transform.position, Quaternion.identity);
           
        currentHealth -= damage;
        betterHealthBar.Damage(currentHealth, maxHealth, (int) damage);
        animator.SetBool("Hit", true);
        gameObject.GetComponent<EnemyAI>().rotated = false;
        if (playerStats.extraHealthOnHit)
        {
            playerStats.UpgradePlayerHealth((int)playerUpgradeHandler.flatExtraHealthOnHit);
        }
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
        }
        enemyAI.LiterallyJustDiasbleTheDamnCOllider();
    }

    public void LiterallyJustTakeDamage(float damage)
    {
        currentHealth -= damage;
        betterHealthBar.Damage(currentHealth, maxHealth, (int) damage);
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
        }
    }

    public void TakeDOTHit(int damage)
    {
        
        Animator animator = gameObject.GetComponent<Animator>();
        Instantiate(blood[(int)Random.Range(0, blood.Length)], transform.position, Quaternion.identity);

        currentHealth -= damage;
        betterHealthBar.Damage(currentHealth, maxHealth, (int) damage);
        //animator.SetBool("Hit", true);
        gameObject.GetComponent<EnemyAI>().rotated = false;
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            GetComponent<EnemyManager>().enemyMode = EnemyManager.Mode.dead;
            animator.SetBool("Hit", true);
            animator.SetBool("Dead", true);
        }
        if (GetComponent<EnemyManager>().enemyMode == EnemyManager.Mode.dead) { return; }

        GetComponent<AudioSource>().pitch = 0.8f;
        GetComponent<AudioSource>().Play();
        enemyAI.LiterallyJustDiasbleTheDamnCOllider();
    }

    public void LoseStamina(int amount)
    {
        currentStamina -= amount;
        regenTimer = 0;
    }

    private void RegnerateStamina()
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina < 0)
        {
            currentStamina = 0;
        }

       
        regenTimer += Time.deltaTime;
        if (regenTimer > 2f && currentStamina < maxStamina)
        {
            currentStamina += (staminaRegenRate * (1 + Time.fixedDeltaTime));
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        

    }

}

