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

public class BetterEnemyHealthbar : MonoBehaviour {

    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = .6f;

    private Image barImage;
    private Image damagedBarImage;
    public float damagedHealthShrinkTimer;
    //private HealthSystem healthSystem;

    PlayerStats playerStats;
    private void Awake() {
        barImage = transform.Find("bar").GetComponent<Image>();
        damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();
    }

    private void Start() {
        //SetHealth(GetHealthNormalized(FindObjectOfType<ThisIsTheBoss>().gameObject.GetComponent<EnemyStats>().currentHealth, FindObjectOfType<ThisIsTheBoss>().gameObject.GetComponent<EnemyStats>().maxHealth));
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void Update() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (barImage.fillAmount < damagedBarImage.fillAmount) {
                float shrinkSpeed = 0.5f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    public void Damage(float current, float max)
    {
        print("On Damaged boss happened");
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        SetHealth(GetHealthNormalized(current, max));
    }

    public float GetHealthNormalized(float currenthealth, float maxHealth)
    {
        return (float)currenthealth / maxHealth;
    }

    public void SetHealth(float healthNormalized) {
        barImage.fillAmount = healthNormalized;
    }
}
