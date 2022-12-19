using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mixer;

    private bool fullscreen = true;
    public void ToggleFullscreen()
    {
        if (!fullscreen)
        {
            fullscreen = !fullscreen;
            Screen.fullScreen = fullscreen;
        }
        else
        {
            fullscreen = !fullscreen;
            Screen.fullScreen = fullscreen;
        }
    }
    public void EditEffectVolume(float value)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);
    }
    public void EditMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
