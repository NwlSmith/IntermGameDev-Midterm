using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/11/2020
 * Creator: Nate Smith
 * 
 * Description: The Stencil manager class.
 * Manages the operation of the stencil, collects StencilColliders, and detects when the stencil is complete.
 */
public class Stencil : MonoBehaviour
{
    // Public Variables.
    public List<StencilCollider> colliders;
    public List<StencilCollider> usedColliders;
    public int initCount;
    public GameObject cameraPos;

    public bool showMats = false;
    public Material showMat;

    void OnEnable()
    {
        colliders = new List<StencilCollider>(GetComponentsInChildren<StencilCollider>());
        usedColliders = new List<StencilCollider>();
        initCount = colliders.Count;
    }

    /*
     * Deactivate the given StencilCollider, update text, and determine if the stencil is complete.
     * Called in OnTriggerEnter() in StencilCollider.cs.
     * stencilCollider: The StencilCollider to be deactivated.
     */
    public void RemoveStencilCollider(StencilCollider stencilCollider)
    {
        usedColliders.Add(stencilCollider);
        colliders.Remove(stencilCollider);
        GameManager.instance.UpdateProgressText(colliders.Count, initCount);
        if ((float)colliders.Count / initCount <= .2f && !showMats)
        {
            showMats = true;
            foreach (StencilCollider sc in colliders)
            {
                sc.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        if ((float)colliders.Count / initCount <= .1f)
        {
            GameManager.instance.StencilFinished(cameraPos);
        }
    }
}
