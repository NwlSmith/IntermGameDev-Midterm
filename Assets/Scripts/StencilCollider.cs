using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilCollider : MonoBehaviour
{
    public bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DrawLine>() != null)
        {
            if (active)
            {
                active = false;
                Debug.Log(name + " was activated");
                GetComponentInParent<Stencil>().RemoveStencilCollider(this);
            }
        }
    }
}
