using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreationMenu : MonoBehaviour
{
    [Header("Relics")]
    public RelicItem[] UnlockedRelics;
    public RelicItem extraSoul;   
    public RelicItem extraLife;
    public RelicItem soulGenerator;
    public RelicItem aceharmonicon;
    public RelicItem dotDebuff;
    public RelicItem extraDamageOnUndamaged;
    public RelicItem extraXP;
    public RelicItem attackingReducesHealth;
    public RelicItem glass;
    public RelicItem soulOnHit;
    public RelicItem startAtLevel3;

    private int selectedRelic = 0;
    public Image relicImage;
    public TextMeshProUGUI relicName;
    public TextMeshProUGUI relicDesc;

    [Header("Invocations")]
    public RelicItem[] unlockedSpells;
    public RelicItem weaponBuff;
    public RelicItem immolation;
    public RelicItem soulForm;
    public RelicItem soulToDamage;

    private int selectedSpell = 0;
    public Image spellImage;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellDesc;
    
    [Header("Weapons")]
    //public WeaponItem[] UnlockedWeapons;
    public GameObject[] playerWeapons;
    public WeaponItem bigSword;
    public WeaponItem bigMace;
    public WeaponItem sword;
    public WeaponItem mace;

    private int selectedWeapon = 0;
    public Image weaponImage;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDesc;



    [Header("Other")]
    [Space(10)]
    public GameObject loadingPanel;

    public Button relicPurchaseButton;
    public Button weaponPurchaseButton;
    public Button spellPurchaseButton;
    public Button finishCreation;
    public TextMeshProUGUI soulsDisplayText;

    UnlockedItemManager unlockedItemManager;
    GameSaveHandler gameSaveHandler;

    private void Start()
    {
        playerWeapons = FindObjectOfType<WeaponEquipHandler>().UpdateWeaponItems();
        unlockedItemManager = FindObjectOfType<UnlockedItemManager>();
        gameSaveHandler = FindObjectOfType<GameSaveHandler>();

        gameSaveHandler.LoadGame();

        extraSoul.unlocked = unlockedItemManager.unlockedExtraSoulsRing;
        weaponBuff.unlocked = unlockedItemManager.unlockedSoulfire;
        extraLife.unlocked = unlockedItemManager.unlockedExtraLivesRing;
        soulGenerator.unlocked = unlockedItemManager.unlockedSoulGenerator;
        aceharmonicon.unlocked = unlockedItemManager.unlockedDoubleUpgrades;
        dotDebuff.unlocked = unlockedItemManager.unlockedDOTRelic;
        extraDamageOnUndamaged.unlocked = unlockedItemManager.unlockedExtraDamageOnUndamaged;
        extraXP.unlocked = unlockedItemManager.unlockedExtraXP;

        attackingReducesHealth.unlocked = unlockedItemManager.unlockedAttackingReducesHealth;
        glass.unlocked = unlockedItemManager.unlockedGlass;
        immolation.unlocked = unlockedItemManager.unlockedImmolation;
        soulForm.unlocked = unlockedItemManager.unlockedSoulForm;
        soulOnHit.unlocked = unlockedItemManager.unlockedSoulOnHit;
        soulToDamage.unlocked = unlockedItemManager.unlockedSoulToDamage;
        startAtLevel3.unlocked = unlockedItemManager.unlockedStartAtLevel3;

        UnlockedRelics[0] = extraSoul;
        UnlockedRelics[1] = extraLife;
        UnlockedRelics[2] = soulGenerator;
        UnlockedRelics[3] = aceharmonicon;
        UnlockedRelics[4] = dotDebuff;
        UnlockedRelics[5] = extraDamageOnUndamaged;
        UnlockedRelics[6] = extraXP;
        UnlockedRelics[7] = attackingReducesHealth;
        UnlockedRelics[8] = glass;
        UnlockedRelics[9] = soulOnHit;
        UnlockedRelics[10] = startAtLevel3;

        unlockedSpells[0] = weaponBuff;
        unlockedSpells[1] = immolation;
        unlockedSpells[2] = soulForm;
        unlockedSpells[3] = soulToDamage;

        sword.unlocked = unlockedItemManager.unlockedSword;
        mace.unlocked = unlockedItemManager.unlockedMace;
        bigSword.unlocked = unlockedItemManager.unlockedBigSword;
        bigMace.unlocked = unlockedItemManager.unlockedBigMace;

        var musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>();
        StartCoroutine(FadeAudioSource.StartFade(GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>(), 3, 1));
        if (musicPlayer.isPlaying != true)
        {
            musicPlayer.Play();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;

        Debug.Log(Application.persistentDataPath);

        
        UpdateSelectedWeaponDisplay(selectedWeapon);
        UpdateSelectedRelicDisplay(selectedRelic);
        UpdateSelectedSpellDisplay(selectedSpell);
    }

    private void Update()
    {
        soulsDisplayText.text = "Shards: " + unlockedItemManager.totalSouls;
    }

    public void NextWeaponItem()
    {
        UpdateWeaponPreview(selectedWeapon, false);
        selectedWeapon++;
        if (selectedWeapon >= playerWeapons.Length)
        {
            selectedWeapon = 0;
        }
        UpdateSelectedWeaponDisplay(selectedWeapon);
    }
    public void PrevWeaponItem()
    {
        UpdateWeaponPreview(selectedWeapon, false);
        selectedWeapon--;
        if (selectedWeapon < 0)
        {
            selectedWeapon = playerWeapons.Length - 1;
        }
        UpdateSelectedWeaponDisplay(selectedWeapon);
    }
    public void NextRelicItem()
    {
        selectedRelic++;
        if (selectedRelic >= UnlockedRelics.Length)
        {
            selectedRelic = 0;
        }
        UpdateSelectedRelicDisplay(selectedRelic);
    }
    public void PrevRelicItem()
    {
        selectedRelic--;
        if (selectedRelic < 0)
        {
            selectedRelic = UnlockedRelics.Length - 1;
        }
        UpdateSelectedRelicDisplay(selectedRelic);
    }
    public void NextSpellItem()
    {
        selectedSpell++;
        if (selectedSpell >= unlockedSpells.Length)
        {
            selectedSpell = 0;
        }
        UpdateSelectedSpellDisplay(selectedSpell);
    }
    public void PrevSpellItem()
    {
        selectedSpell--;
        if (selectedSpell < 0)
        {
            selectedSpell = unlockedSpells.Length - 1;
        }
        UpdateSelectedSpellDisplay(selectedSpell);
    }

    private void UpdateSelectedSpellDisplay(int selectedSpell)
    {
        spellName.text = unlockedSpells[selectedSpell].itemName;
        spellDesc.text = unlockedSpells[selectedSpell].itemDesc;
        if (unlockedSpells[selectedSpell].unlocked != true)
        {
            spellPurchaseButton.gameObject.SetActive(true);
            finishCreation.interactable = false;
            spellPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase " + unlockedSpells[selectedSpell].unlockCost;
        }
        else
        {
            spellPurchaseButton.gameObject.SetActive(false);
            finishCreation.interactable = true;
        }
    }

    private void UpdateSelectedRelicDisplay(int selectedRelic)
    {
        //relicImage.sprite = UnlockedRelics[selectedRelic].itemIcon;
        relicName.text = UnlockedRelics[selectedRelic].itemName;
        relicDesc.text = UnlockedRelics[selectedRelic].itemDesc;
        if (UnlockedRelics[selectedRelic].unlocked != true)
        {
            relicPurchaseButton.gameObject.SetActive(true);
            finishCreation.interactable = false;
            relicPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase " + UnlockedRelics[selectedRelic].unlockCost;
        }
        else
        {
            relicPurchaseButton.gameObject.SetActive(false);
            finishCreation.interactable = true;
        }
    }

    private void UpdateSelectedWeaponDisplay(int selectedWeapon)
    {
        //weaponImage.sprite = playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.itemIcon;
        weaponDesc.text = playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.itemDesc;
        weaponName.text = playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.itemName;

        if (playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlocked != true)
        {
            weaponPurchaseButton.gameObject.SetActive(true);
            finishCreation.interactable = false;
            weaponPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase " + playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlockCost;
        }
        else
        {
            weaponPurchaseButton.gameObject.SetActive(false);
            finishCreation.interactable = true;
        }

        UpdateWeaponPreview(selectedWeapon, true);
    }

    private void UpdateWeaponPreview(int selectedWeapon, bool enabled)
    {
        playerWeapons[selectedWeapon].GetComponent<MeshRenderer>().enabled = enabled;
    }

    public void DoneButton()
    {
        FinishCreation(selectedWeapon, selectedRelic, selectedSpell);
    }
    private void FinishCreation(int selectedWeapon, int selectedRelic, int selectedSpell)
    {
        GameManager.selectedStartingRelic = selectedRelic;
        GameManager.selectedStartingWeapon = selectedWeapon;
        GameManager.selectedStartingSpell = selectedSpell;

        GameManager.selectedStartingRelicData = UnlockedRelics[selectedRelic];
        GameManager.selectedStartingWeaponData = playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem;
        GameManager.selectedStartingSpellData = unlockedSpells[selectedSpell];

        gameSaveHandler.SaveGame(unlockedItemManager);
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(2);
    }

    public void PurchaseSpell()
    {
        if (unlockedItemManager.totalSouls < unlockedSpells[selectedSpell].unlockCost)
        {
            return;
        }
        unlockedItemManager.totalSouls -= unlockedSpells[selectedSpell].unlockCost;
        switch (unlockedSpells[selectedSpell].itemName)
        {
            case "Soulfire Weapon":
                unlockedItemManager.unlockedSoulfire = true;
                weaponBuff.unlocked = true;
                unlockedSpells[selectedSpell].unlocked = true;
                print("purchased soulfire spell");
                break;
            case "Touch of Darkness":
                unlockedItemManager.unlockedImmolation = true;
                immolation.unlocked = true;
                unlockedSpells[selectedSpell].unlocked = true;
                print("Purchased immolation");
                break;
            case "Soulform":
                unlockedItemManager.unlockedSoulForm = true;
                soulForm.unlocked = true;
                unlockedSpells[selectedSpell].unlocked = true;
                print("Purchased Soulform");
                break;
            case "Essence Flux":
                unlockedItemManager.unlockedSoulToDamage = true;
                soulToDamage.unlocked = true;
                unlockedSpells[selectedSpell].unlocked = true;
                print("Purchased Essence Flux");
                break;
        }
        UpdateSelectedSpellDisplay(selectedSpell);
        gameSaveHandler.SaveGame(unlockedItemManager);
    }

    public void PurchaseRelic()
    {
        if (unlockedItemManager.totalSouls < UnlockedRelics[selectedRelic].unlockCost)
        {
            return;
        }
        unlockedItemManager.totalSouls -= UnlockedRelics[selectedRelic].unlockCost;
        switch (UnlockedRelics[selectedRelic].itemName)
        {
            case "Philospher's Ring":
                unlockedItemManager.unlockedExtraLivesRing = true;
                extraLife.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("purchased philospoger's ring");
                break;
            case "Ring of Gluttony":
                unlockedItemManager.unlockedExtraSoulsRing = true;
                extraSoul.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("purchased gluttony ring");
                break;
            case "Siphon":
                unlockedItemManager.unlockedSoulGenerator = true;
                soulGenerator.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased soul siphon");
                break;
            case "Necroharmonicon":
                unlockedItemManager.unlockedDoubleUpgrades = true;
                aceharmonicon.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Aceharmonicon");
                break;
            case "Soulplague":
                unlockedItemManager.unlockedDOTRelic = true;
                dotDebuff.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Soulplague");
                break;
            case "Bloodseeker":
                unlockedItemManager.unlockedExtraDamageOnUndamaged = true;
                extraDamageOnUndamaged.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased bloodseeker");
                break;
            case "Mindshreeker":
                unlockedItemManager.unlockedExtraXP = true;
                extraXP.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased mindshreeker");
                break;
            case "Witchbane":
                unlockedItemManager.unlockedAttackingReducesHealth = true;
                attackingReducesHealth.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Witchbane");
                break;
            case "Glass Shard":
                unlockedItemManager.unlockedGlass = true;
                glass.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Glass Shard");
                break;
            case "Soulcutter":
                unlockedItemManager.unlockedSoulOnHit = true;
                soulOnHit.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Soulcutter");
                break;
            case "Infernal Pact":
                unlockedItemManager.unlockedStartAtLevel3 = true;
                startAtLevel3.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased Infernal Pact");
                break;
        }
        UpdateSelectedRelicDisplay(selectedRelic);
        gameSaveHandler.SaveGame(unlockedItemManager);
    }

    public void PurchaseWeapon()
    {
        if (unlockedItemManager.totalSouls < playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlockCost)
        {
            return;
        }
        unlockedItemManager.totalSouls -= playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlockCost;
        switch (playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.itemName)
        {
            case "Greatsword":
                unlockedItemManager.unlockedBigSword = true;
                bigSword.unlocked = true;
                playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlocked = true;
                print("purchased greatsword");
                break;
            case "Warhammer":
                unlockedItemManager.unlockedBigMace = true;
                bigMace.unlocked = true;
                playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlocked = true;
                print("purchased warhammer");
                break;
            case "Mace":
                unlockedItemManager.unlockedMace = true;
                mace.unlocked = true;
                playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlocked = true;
                print("purchased mace");
                break;
            case "Sword":
                unlockedItemManager.unlockedSword = true;
                sword.unlocked = true;
                playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem.unlocked = true;
                print("purchased sword");
                break;
        }
        UpdateSelectedWeaponDisplay(selectedWeapon);
        gameSaveHandler.SaveGame(unlockedItemManager);
    }

}
