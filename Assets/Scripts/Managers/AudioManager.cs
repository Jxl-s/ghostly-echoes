using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource; 
    public AudioSource soundEffect;
    public AudioClip[] musicClips;
    public AudioClip[] effectsClips;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeSound(int index) {
        AudioClip clip = musicClips[index];
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayEffect(int index = 0) {    
        AudioClip clip = effectsClips[index];
        soundEffect.clip = clip;
        soundEffect.Play();
    }

    public void ToggleMusic(bool toggle) {
        if (!toggle)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }
    
}
