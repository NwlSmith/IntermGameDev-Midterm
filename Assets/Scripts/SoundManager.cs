using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float needlepitchrange = .05f;

    private AudioSource audioSource;
    private Rigidbody rb;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            audioSource.volume = 1f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            audioSource.volume = 0f;
        }
    }
}
