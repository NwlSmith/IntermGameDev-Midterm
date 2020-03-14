using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/11/2020
 * Creator: Nate Smith
 * 
 * Description: The StencilCollider class.
 * Detects collisions with this part of the stencil and informs the manager class.
 */
public class StencilCollider : MonoBehaviour
{
    // Public Variables.
    public bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        // If the InkProjector GameObject enters this trigger, and it is active.
        if (other.GetComponent<DrawLine>() != null)
        {
            if (active)
            {
                // Deactivate, and inform the manager class.
                active = false;
                Debug.Log(name + " was activated");
                GetComponentInParent<Stencil>().RemoveStencilCollider(this);
            }
        }
    }
}
