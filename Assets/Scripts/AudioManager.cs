using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/10/2020
 * Creator: Nate Smith
 * 
 * Description: The Audio Manager Class.
 * Is a single instance static object - There should only be 1 AudioManager.
 * Controls music, sound effects, ambient sounds.
 */
public class AudioManager : MonoBehaviour
{
    // Static instance of the object.
    public static AudioManager instance = null;

    public bool introSequence = false;

    // Public objects.
    public AudioClip tattooGun;
    public AudioClip ambientPain;
    public AudioClip[] pain;
    public AudioClip[] gush;
    public AudioClip splash;
    public AudioClip music;
    public AudioClip button;
    public AudioClip finished;

    // Starting volumes for each sound effect.
    [Range(0.0f, 1.0f)]
    public float tattooGunVol = .7f;
    [Range(0.0f, 1.0f)]
    public float ambientPainVol = 1f;
    [Range(0.0f, 1.0f)]
    public float painVol = .7f;
    [Range(0.0f, 1.0f)]
    public float gushVol = 1f;
    [Range(0.0f, 1.0f)]
    public float splashVol = 1f;
    [Range(0.0f, 1.0f)]
    public float musicVol = 1f;

    // Private AudioSources.
    private AudioSource tattooGunAS;
    private AudioSource ambientPainAS;
    private AudioSource painAS;
    private AudioSource gushAS;
    private AudioSource splashAS;
    private AudioSource musicAS;
    private AudioSource buttonAS;
    private AudioSource finishedAS;

    void Start()
    {
        // Ensure that there is only one instance of the AudioManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Set up each AudioSource and their starting properties.

        tattooGunAS = gameObject.AddComponent<AudioSource>();
        tattooGunAS.playOnAwake = false;
        tattooGunAS.loop = true;
        tattooGunAS.volume = 0f;

        ambientPainAS = gameObject.AddComponent<AudioSource>();
        ambientPainAS.playOnAwake = false;
        ambientPainAS.loop = true;
        ambientPainAS.volume = 0f;

        painAS = gameObject.AddComponent<AudioSource>();
        painAS.playOnAwake = false;
        painAS.loop = false;
        painAS.volume = painVol;

        gushAS = gameObject.AddComponent<AudioSource>();
        gushAS.playOnAwake = false;
        gushAS.loop = false;
        gushAS.volume = gushVol;

        splashAS = gameObject.AddComponent<AudioSource>();
        splashAS.playOnAwake = false;
        splashAS.loop = false;
        splashAS.volume = splashVol;

        musicAS = gameObject.AddComponent<AudioSource>();
        musicAS.playOnAwake = true;
        musicAS.loop = true;
        musicAS.volume = musicVol;

        buttonAS = gameObject.AddComponent<AudioSource>();
        buttonAS.playOnAwake = false;
        buttonAS.loop = false;

        finishedAS = gameObject.AddComponent<AudioSource>();
        finishedAS.playOnAwake = false;
        finishedAS.loop = false;

        // Assign clips and play.
        tattooGunAS.clip = tattooGun;
        tattooGunAS.Play();

        ambientPainAS.clip = ambientPain;
        ambientPainAS.Play();

        splashAS.clip = splash;

        musicAS.clip = music;
        musicAS.Play();

        buttonAS.clip = button;
        finishedAS.clip = finished;

        // AudioManager should be retained between scenes.
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!introSequence)
        {
            // If the player clicks while the game is not paused or showing finished product, play tattoo sounds
            if (!GameManager.instance.levelTransition &&
                Input.GetMouseButtonDown(0) &&
                !GameManager.instance.pauseText.enabled &&
                !GameManager.instance.screenshotButton.enabled)
            {
                tattooGunAS.volume = tattooGunVol;
                ambientPainAS.volume = ambientPainVol;
            }
            // Otherwise, turn off sounds.
            else if (Input.GetMouseButtonUp(0))
            {
                tattooGunAS.volume = 0f;
                ambientPainAS.volume = 0f;
            }
        }
    }

    /*
     * Play random bleeding and pain sounds.
     * Invoked in GenerateBlood() in DrawLine.cs
     */
    public void BloodSound()
    {
        painAS.clip = pain[Random.Range(0, pain.Length)];
        painAS.Play();

        gushAS.clip = gush[Random.Range(0, gush.Length)];
        gushAS.Play();
    }

    public void PlayButtonSound()
    {
        buttonAS.Play();
    }

    public void PlayFinishedSound()
    {
        finishedAS.Play();
    }
}
