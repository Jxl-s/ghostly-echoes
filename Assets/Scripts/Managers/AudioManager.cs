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
        audioSource.clip = musicClips[index];
        audioSource.Play();
    }
    public void PlayEffect(int index = 0) {
        soundEffect.clip = effectsClips[index];
        soundEffect.Play();
    }
    
}
