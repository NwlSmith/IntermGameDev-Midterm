using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/21/2020
 * Creator: Nate Smith
 * 
 * Description: The Ink Chooser class.
 * Assigns the given inkMaterial to the tattoo machine on collisions.
 */
public class InkChooser : MonoBehaviour
{
    // Public Variable.
    public Material inkMaterial;

    /*
     * When the DrawLine component enters this trigger area, assign inkMaterial to the current line.
     */
    private void OnTriggerEnter(Collider other)
    {
        DrawLine otherDL = other.GetComponent<DrawLine>();
        if (otherDL)
        {
            otherDL.curInk = inkMaterial;
        }
    }
}
