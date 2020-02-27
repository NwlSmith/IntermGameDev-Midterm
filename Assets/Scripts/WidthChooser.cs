using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthChooser : MonoBehaviour
{

    public float width;

    private void OnTriggerEnter(Collider other)
    {
        DrawLine otherDL = other.GetComponent<DrawLine>();
        if (otherDL)
        {
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
