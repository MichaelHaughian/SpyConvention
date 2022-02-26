using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    AudioSource audioSource;

    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip radarSound;
    [SerializeField] AudioClip poisonSound;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGunShot() {
        audioSource.PlayOneShot(gunShot);
    }

    public void PlayRadar() {
        audioSource.PlayOneShot(radarSound);
    }

    public void PlayPoisonSound() {
        audioSource.PlayOneShot(poisonSound);
    }
}
