using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] attackSoundEffects;
    public AudioClip[] deathSoundEffects;
    public AudioClip[] walkSoundEffects;
    public AudioClip[] playerHurtSoundEffects;
    public AudioClip[] playerRollSoundEffects;
    public AudioSource audioSource;
    // Start is called before the first frame update
    [ExecuteAlways]
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void AttackSound()
    {
        /*if (!audioSource.isPlaying) */
            audioSource.PlayOneShot(attackSoundEffects[Random.Range(0,attackSoundEffects.Length)]);
    }
    public void RollSound()
    {
        /*if (!audioSource.isPlaying) */
        //AttackSound();
        audioSource.PlayOneShot(attackSoundEffects[Random.Range(0, attackSoundEffects.Length)]);
        audioSource.PlayOneShot(playerRollSoundEffects[0]);
    }

    public void PlayerHurtSound()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(playerHurtSoundEffects[Random.Range(0, playerHurtSoundEffects.Length)]);
    }

    public void DeathSound()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(deathSoundEffects[Random.Range(0, deathSoundEffects.Length)]);
    }


    public void MoveSound()
    {
            audioSource.PlayOneShot(walkSoundEffects[Random.Range(0, walkSoundEffects.Length)]);
    }
}
