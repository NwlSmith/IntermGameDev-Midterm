using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The Destroy Object class.
 * Destroys the current object, in this case, the BloodParticles object, after a time.
 */
public class DestroyObject : MonoBehaviour
{
    // The time before the GameObject is destroyed.
    public float destroyTime = 0f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
