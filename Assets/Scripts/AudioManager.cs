using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioClip tattooGun;
    public AudioClip ambientPain;
    public AudioClip[] pain;
    public AudioClip[] gush;
    public AudioClip splash;
    public AudioClip music;

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

    private AudioSource tattooGunAS;
    private AudioSource ambientPainAS;
    private AudioSource painAS;
    private AudioSource gushAS;
    private AudioSource splashAS;
    private AudioSource musicAS;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

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

        tattooGunAS.clip = tattooGun;
        tattooGunAS.Play();

        ambientPainAS.clip = ambientPain;
        ambientPainAS.Play();

        splashAS.clip = splash;

        musicAS.clip = music;
        musicAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.pauseText.enabled == false)
        {
            tattooGunAS.volume = tattooGunVol;
            ambientPainAS.volume = ambientPainVol;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            tattooGunAS.volume = 0f;
            ambientPainAS.volume = 0f;
        }
    }

    public void BloodSound()
    {
        painAS.clip = pain[Random.Range(0, pain.Length)];
        painAS.Play();

        gushAS.clip = gush[Random.Range(0, gush.Length)];
        gushAS.Play();
    }
}
