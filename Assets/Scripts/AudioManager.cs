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

    public int loopNum = 0;

    // Public objects.
    public AudioClip tattooGun;
    public AudioClip ambientPain;
    public AudioClip[] pain;
    public AudioClip[] gush;
    public AudioClip splash;
    public AudioClip[] loops;
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

        musicAS.clip = loops[0];
        musicAS.Play();

        buttonAS.clip = button;
        finishedAS.clip = finished;

        // AudioManager should be retained between scenes.
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // If the player stops clicking, the tattoo gun should never make noise.
        if (Input.GetMouseButtonUp(0))
        {
            tattooGunAS.volume = 0f;
            ambientPainAS.volume = 0f;
        }
    }

    /*
     * Plays the next loop in the AudioClip array.
     * Invoked in the Level1to2 class in () and Level2Transitions() when transitioning between stencils.
     */
    public void TransitionTrack()
    {
        loopNum++;
        loopNum = Mathf.Min(loopNum, loops.Length - 1);
        NextTrack();
    }

    /*
     * Plays the specified loop in the AudioClip array.
     * Invoked in the Level1to2 class in () and Level2Transitions() when transitioning between stencils.
     */
    public void TransitionTrack(int newNum)
    {
        loopNum = newNum;
        loopNum = Mathf.Min(loopNum, loops.Length - 1);
        NextTrack();
    }

    private void NextTrack()
    {
        float time = musicAS.time;
        musicAS.clip = loops[loopNum];
        musicAS.time = time;
        musicAS.Play();
    }

    /*
     * Play the tattooGun sound and ambient pain sound.
     * Invoked in OnTriggerEnter() in DrawLine.cs
     */
    public void PlayMachineSound()
    {
        tattooGunAS.volume = tattooGunVol;
        ambientPainAS.volume = ambientPainVol;
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
