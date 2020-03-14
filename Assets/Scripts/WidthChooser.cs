using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The Width chooser class.
 * Assigns the given width to the tattoo gun on collisions, and updates the needle models.
 */
public class WidthChooser : MonoBehaviour
{
    // Public Variable.
    public float width;

    private void OnTriggerEnter(Collider other)
    {
        // If the InkProjector GameObject enters this trigger.
        DrawLine otherDL = other.GetComponent<DrawLine>();
        if (otherDL)
        {
            // Change the width of the interacting InkProjector and activate/deactivate meshes.
            otherDL.curWidth = width;
            if (width == .025f)
            {
                otherDL.needleMeshes[1].enabled = false;
                otherDL.needleMeshes[2].enabled = false;
            }
            else if (width == .05f)
            {
                otherDL.needleMeshes[1].enabled = true;
                otherDL.needleMeshes[2].enabled = false;
            }
            else if (width == .1f)
            {
                otherDL.needleMeshes[1].enabled = true;
                otherDL.needleMeshes[2].enabled = true;
            }
        }
    }
}
