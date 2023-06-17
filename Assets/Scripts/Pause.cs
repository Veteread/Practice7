using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    private bool paused;
    public AudioMixerGroup Mixer;
    public void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        paused = !paused;
    }

    public void ChangeVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("SoundsVolume", Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat("SoundsVolume", volume);
    }
}
