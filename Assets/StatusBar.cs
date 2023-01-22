using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public StatusIcon[] statusIcons = new StatusIcon[6];
    public Sprite[] sprites = new Sprite[6];
    private EnemyDebuffHandler debuffHandler;
    public GameObject[] statusEffectParticles;
    public EnemyManager enemyManager;
    private void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    private void Update()
    {
        if (enemyManager && enemyManager.enemyMode == EnemyManager.Mode.dead)
        {
            Destroy(gameObject);
            var particles = GameObject.FindGameObjectsWithTag("StatusParticle");
            foreach (GameObject obj in particles)
            {
                Destroy(obj);
            }
        }
    }
    // Update is called once per frame
    public void AddStatus(StatusEnum.DisplayedDebuff debuff)
    {
       foreach (StatusIcon icon in statusIcons)
       {
            if (!icon.isDisplaying)
            {
                icon.isDisplaying = true;
                switch (debuff)
                {
                    case StatusEnum.DisplayedDebuff.Poison:
                        icon.statusImage.sprite = sprites[0];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Poison;
                        break;
                    case StatusEnum.DisplayedDebuff.Ice:
                        icon.statusImage.sprite = sprites[1];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Ice;
                        break;
                    case StatusEnum.DisplayedDebuff.Bleed:
                        icon.statusImage.sprite = sprites[2];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Bleed;
                        break;
                    case StatusEnum.DisplayedDebuff.Rot:
                        icon.statusImage.sprite = sprites[3];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Rot;
                        break;
                    case StatusEnum.DisplayedDebuff.Hemmorage:
                        icon.statusImage.sprite = sprites[4];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Hemmorage;
                        break;
                    case StatusEnum.DisplayedDebuff.Stun:
                        icon.statusImage.sprite = sprites[5];
                        icon.displayedDebuff = StatusEnum.DisplayedDebuff.Stun;
                        break;
                }
                break;
            }
       }
    }
}
