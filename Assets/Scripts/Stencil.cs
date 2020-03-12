using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stencil : MonoBehaviour
{

    /*
     * detect colliders in children
     * add them to list
     * if tattoo gun collides with colliders, remove from list
     * check if all colliders are removed,
     * if yes, you win or something
     */

    public List<StencilCollider> colliders;
    public List<StencilCollider> usedColliders;
    public int initCount;
    public GameObject cameraPos;

    void Start()
    {
        colliders = new List<StencilCollider>(GetComponentsInChildren<StencilCollider>());
        usedColliders = new List<StencilCollider>();
        initCount = colliders.Count;
    }

    public void RemoveStencilCollider(StencilCollider stencilCollider)
    {
        usedColliders.Add(stencilCollider);
        colliders.Remove(stencilCollider);
        GameManager.instance.UpdateProgressText(colliders.Count, initCount);
        if (colliders.Count <= 0)
        {
            Debug.Log("All Stencils Cleared");
            GetComponent<MeshRenderer>().enabled = false;
            GameManager.instance.StencilFinished(cameraPos);
        }
    }
}
