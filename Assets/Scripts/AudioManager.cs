using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] AudioSource musicAudio = null;
    [SerializeField] AudioSource sfxAudio = null;

    [Header("Clips")]
    [SerializeField] AudioClip[] musics = null;
    [SerializeField] AudioClip[] shortPenguinNoises = null;
    [SerializeField] AudioClip fallingPenguinClip = null;

    public static AudioManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        musicAudio.clip = musics[Random.Range(0, musics.Length)];
        musicAudio.Play();
    }

    public void PlayPenguinFallingSound()
    {
        sfxAudio.pitch = Random.Range(0.95f, 1.05f);
        sfxAudio.PlayOneShot(fallingPenguinClip);
    }

    public void PlayShortPenguinSound()
    {
        sfxAudio.pitch = Random.Range(0.95f, 1.05f);
        sfxAudio.PlayOneShot(shortPenguinNoises[Random.Range(0, shortPenguinNoises.Length)]);
    }
}
