using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminBar;
    public Slider soullyBar;

    public Image itemSprite;
    public Text itemText;

    private RawImage soulBarRawImage;
    public void Start()
    {
        soulBarRawImage = soullyBar.gameObject.GetComponentInChildren<RawImage>();
    }
    private void Update()
    {
        Rect uvRect = soulBarRawImage.uvRect;
        uvRect.x += 1f * Time.deltaTime;
        soulBarRawImage.uvRect = uvRect;
    }
    public void UpdateStatsUI(float stamina, float health, float soul, int maxHealth, int maxStamina, PlayerEquipment playerEquipment)
    {   
        healthBar.maxValue = maxHealth;
        staminBar.maxValue = maxStamina;
        soullyBar.maxValue = maxHealth;

        healthBar.value = health;
        staminBar.value = stamina;
        soullyBar.value = health + (soul);
    }

    public void UpdateStatsUI(float health, int maxHealth)
    {
        healthBar.value = health;
    }
    public void UpdateStatsUI(float stamina, float maxStamina)
    {
        staminBar.value = stamina;
    }
    public void UpdateStatsUI(int health, float maxHealth, float soul)
    {
        healthBar.value = health;
        soullyBar.value = health + (soul);
        soullyBar.maxValue = maxHealth;
    }

   

}
