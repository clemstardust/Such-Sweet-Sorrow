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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.2f;
    }

    public void SelectUpgrade()
    {
        //print("Upgrade selected!");
        //print(GameObject.FindGameObjectWithTag("Player").name);
        //print(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgradeHandler>().enabled);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgradeHandler>().SendMessage(upgradeItem.function);
        //FindObjectOfType<PlayerUpgradeHandler>().SendMessage(upgradeItem.function);
        //print("Upgrade selected!");
        upgradeMenu.gameObject.SetActive(false);
    }
}
