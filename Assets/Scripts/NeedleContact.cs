using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleContact : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("Hewwo");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Time.time + " Needle in");
    }
}
