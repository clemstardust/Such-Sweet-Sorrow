using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{

    public GameObject bossHealthbar;
    public EnemyStats boss;
    public Collider triggerCollider;
    public Collider triggerCollider2;
    // Start is called before the first frame update
    void Start()
    {
        bossHealthbar = GameObject.FindGameObjectWithTag("BossHealthbar");
        bossHealthbar.GetComponent<Slider>().maxValue = boss.maxHealth;
        bossHealthbar.SetActive(false);
    }

    private void Update()
    {
        bossHealthbar.GetComponent<Slider>().value = boss.currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossHealthbar.SetActive(true);
            GameObject.FindGameObjectWithTag("MusicPlayerBoss").GetComponent<AudioSource>().Play();
            StartCoroutine(FadeAudioSource.StartFade(GameObject.FindGameObjectWithTag("MusicPlayerBoss").GetComponent<AudioSource>(), 3, 1));
            //StartCoroutine(FadeAudioSource.StartFade(GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>(), 3, 0));
            //GameObject.FindGameObjectWithTag("MusicPlayerBackground").GetComponent<AudioSource>().Pause();
            triggerCollider.enabled = false;
            triggerCollider2.enabled = false;
        }
    }
}
