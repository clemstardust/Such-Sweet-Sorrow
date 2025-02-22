using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public UpgradeItem upgradeItem;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI desc;
    public UpgradeMenu upgradeMenu;
    //public Image image;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerStats>().currentHealth > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }

    }

    public void SelectUpgrade()
    {
        //print("Upgrade selected!");
        //print("Found player: " + GameObject.FindGameObjectWithTag("Player").name);
        // print("Found upgrade handler: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgradeHandler>().enabled);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgradeHandler>().SendMessage(upgradeItem.function);
        FindObjectOfType<PlayerUpgradeHandler>().SendMessage(upgradeItem.function);
        //FindObjectOfType<PlayerUpgradeHandler>().SendMessage(upgradeItem.function);
        //print("Upgrade selected!");
        upgradeMenu.gameObject.SetActive(false);
    }
}
