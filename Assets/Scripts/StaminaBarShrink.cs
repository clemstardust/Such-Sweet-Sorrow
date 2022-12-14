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

public class StaminaBarShrink : MonoBehaviour {

    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = .6f;

    private Image barImage;
    private Image damagedBarImage;
    public float damagedHealthShrinkTimer;
    public RectTransform rectTransform;
    //private HealthSystem healthSystem;

    PlayerStats playerStats;
    private void Awake() {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        SetHealth(GetHealthNormalized(FindObjectOfType<PlayerStats>().currentStamina, FindObjectOfType<PlayerStats>().maxStamina));
        damagedBarImage.fillAmount = barImage.fillAmount;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (barImage.fillAmount < damagedBarImage.fillAmount) {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
        rectTransform.sizeDelta = new Vector2(playerStats.maxStamina * 1.5f, 10);

    }

    public void Regenerate(float current, float max)
    {
        //print("On regenerate happened");
        SetHealth(GetHealthNormalized(current, max));
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    public void Degenenerate(float current, float max)
    {
        //print("On degenerate happened");
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        SetHealth(GetHealthNormalized(current, max));
    }

    public float GetHealthNormalized(float currentStamina, float maxStamina)
    {
        return (float)currentStamina / maxStamina;
    }

    private void SetHealth(float staminaNormalized) {
        barImage.fillAmount = staminaNormalized;
    }
}
