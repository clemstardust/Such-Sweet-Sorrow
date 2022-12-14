/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarShrink : MonoBehaviour {

    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = .6f;

    private Image barImage;
    private Image damagedBarImage;
    public float damagedHealthShrinkTimer;
    public RectTransform rectTransform;
    public RectTransform soulBarTransform;
    public PlayerUpgradeHandler playerUpgradeHandler;

    PlayerStats playerStats;
    private void Awake() {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        SetHealth(GetHealthNormalized(FindObjectOfType<PlayerStats>().currentHealth, FindObjectOfType<PlayerStats>().maxHealth));
        damagedBarImage.fillAmount = barImage.fillAmount;
        playerStats = FindObjectOfType<PlayerStats>();
        playerUpgradeHandler = FindObjectOfType<PlayerUpgradeHandler>();
    }

    private void Update() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (barImage.fillAmount < damagedBarImage.fillAmount) {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
        rectTransform.sizeDelta = new Vector2(playerStats.maxHealth / 3, 10);
        soulBarTransform.sizeDelta = new Vector2((playerStats.maxHealth +  playerUpgradeHandler.maxSoulUpLevel) / 3, 16);

    }

    public void Heal(float current, float max)
    {
        //print("On heal happened");
        SetHealth(GetHealthNormalized(current, max));
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    public void Damage(float current, float max)
    {
        //print("On Damaged happened");
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        SetHealth(GetHealthNormalized(current, max));
    }

    public float GetHealthNormalized(float currenthealth, float maxHealth)
    {
        return (float)currenthealth / maxHealth;
    }

    private void SetHealth(float healthNormalized) {
        barImage.fillAmount = healthNormalized;
    }
}
