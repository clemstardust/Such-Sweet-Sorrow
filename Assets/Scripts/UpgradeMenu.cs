using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public UpgradeItem[] upgrades;
    public UpgradeSlot[] upgradeSlots;
    public void ReloadMenu()
    {
        foreach (UpgradeSlot slot in upgradeSlots)
        {
            UpgradeItem upgradeItem = upgrades[Random.Range(0, upgrades.Length)];
            slot.upgradeName.text = upgradeItem.itemName;
            slot.desc.text = upgradeItem.itemDesc;
            //slot.image.sprite = upgradeItem.itemIcon;
            slot.upgradeItem = upgradeItem;
        }
    }
}
