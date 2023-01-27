using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusEnum.DisplayedDebuff displayedDebuff;
    public TextMeshProUGUI numStacks;
    public bool isDisplaying = false;
    public Image statusImage;
    private EnemyDebuffHandler debuffHandler;
    private void Start()
    {
        debuffHandler = GetComponentInParent<EnemyDebuffHandler>();
        try
        {
            if (debuffHandler == null) debuffHandler = GameObject.Find("PHBOSS").GetComponent<EnemyDebuffHandler>();
        }
        catch
        {
            if (debuffHandler == null) debuffHandler = GameObject.Find("Swamp Boss").GetComponent<EnemyDebuffHandler>();
        }
        statusImage = GetComponent<Image>();
        numStacks = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (isDisplaying)
        {
            numStacks.enabled = true;
            statusImage.enabled = true;
            switch (displayedDebuff)
            {
                case StatusEnum.DisplayedDebuff.Poison:
                    numStacks.text = "" + debuffHandler.poisonStacks;
                    break;
                case StatusEnum.DisplayedDebuff.Ice:
                    numStacks.text = "" + debuffHandler.iceStacks;
                    break;
                case StatusEnum.DisplayedDebuff.Bleed:
                    numStacks.text = "" + debuffHandler.bleedStacks;
                    break;
                case StatusEnum.DisplayedDebuff.Rot:
                    numStacks.text = "" + debuffHandler.rotStacks;
                    break;
                case StatusEnum.DisplayedDebuff.Hemmorage:
                    numStacks.text = "" + debuffHandler.hemmorageStacks;
                    break;
                case StatusEnum.DisplayedDebuff.Stun:
                    numStacks.text = "" + debuffHandler.stunStacks;
                    break;
            }
        }
        else
        {
            numStacks.enabled = false;
            statusImage.enabled = false;
        }
    }

}
