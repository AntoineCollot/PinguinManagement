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
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void PlayPenguinFallingSound()
    {
        sfxAudio.pitch = Random.Range(0.95f, 1.05f);
        sfxAudio.PlayOneShot(fallingPenguinClip, 0.5f);
    }

    public void PlayShortPenguinSound()
    {
        sfxAudio.pitch = Random.Range(0.95f, 1.05f);
        sfxAudio.PlayOneShot(shortPenguinNoises[Random.Range(0, shortPenguinNoises.Length)]);
    }

    public void PlayMusic(bool value)
    {
        if (value)
            musicAudio.UnPause();
        else
            musicAudio.Pause();
    }
}
