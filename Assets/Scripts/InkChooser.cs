using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkChooser : MonoBehaviour
{

    public Material inkMaterial;

    private void OnTriggerEnter(Collider other)
    {
        DrawLine otherDL = other.GetComponent<DrawLine>();
        if (otherDL)
        {
            otherDL.curInk = inkMaterial;
        }
    }
}
