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
    public RelicItem soulShield;
    public RelicItem weaponBuff;
    public RelicItem extraLife;
    public RelicItem soulGenerator;
    public RelicItem aceharmonicon;
    public RelicItem dotDebuff;

    private int selectedRelic = 0;
    public Image relicImage;
    public TextMeshProUGUI relicName;
    public TextMeshProUGUI relicDesc;
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
    public Button finishCreation;
    public TextMeshProUGUI soulsDisplayText;

    UnlockedItemManager unlockedItemManager;
    GameSaveHandler gameSaveHandler;

    private void Start()
    {
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

        playerWeapons = FindObjectOfType<WeaponEquipHandler>().UpdateWeaponItems();
        unlockedItemManager = FindObjectOfType<UnlockedItemManager>();
        gameSaveHandler = FindObjectOfType<GameSaveHandler>();
        UpdateSelectedWeaponDisplay(selectedWeapon);
        UpdateSelectedRelicDisplay(selectedRelic);

        gameSaveHandler.LoadGame();

        extraSoul.unlocked = unlockedItemManager.unlockedExtraSoulsRing;
        soulShield.unlocked = unlockedItemManager.unlockedSoulshield;
        weaponBuff.unlocked = unlockedItemManager.unlockedSoulfire;
        extraLife.unlocked = unlockedItemManager.unlockedExtraLivesRing;
        soulGenerator.unlocked = unlockedItemManager.unlockedSoulGenerator;
        aceharmonicon.unlocked = unlockedItemManager.unlockedDoubleUpgrades;

        sword.unlocked = unlockedItemManager.unlockedSword;
        mace.unlocked = unlockedItemManager.unlockedMace;
        bigSword.unlocked = unlockedItemManager.unlockedBigSword;
        bigMace.unlocked = unlockedItemManager.unlockedBigMace;

    }

    private void Update()
    {
        soulsDisplayText.text = "Souls: " + unlockedItemManager.totalSouls;
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
        FinishCreation(selectedWeapon, selectedRelic);
    }
    private void FinishCreation(int selectedWeapon, int selectedRelic)
    {
        GameManager.selectedStartingRelic = selectedRelic;
        GameManager.selectedStartingWeapon = selectedWeapon;

        GameManager.selectedStartingRelicData = UnlockedRelics[selectedRelic];
        GameManager.selectedStartingWeaponData = playerWeapons[selectedWeapon].GetComponent<AttackHitboxObject>().weaponItem;

        gameSaveHandler.SaveGame(unlockedItemManager);

        loadingPanel.SetActive(true);
        SceneManager.LoadScene(2);
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
            case "Soulshield":
                unlockedItemManager.unlockedSoulshield = true;
                soulShield.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("purchased soulshield");
                break;
            case "Soulfire Weapon":
                unlockedItemManager.unlockedSoulfire = true;
                soulShield.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("purchased soulfire spell");
                break;
            case "Siphon":
                unlockedItemManager.unlockedSoulGenerator = true;
                soulGenerator.unlocked = true;
                UnlockedRelics[selectedRelic].unlocked = true;
                print("Purchased soul siphon");
                break;
            case "Aceharmonicon":
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
