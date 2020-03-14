using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/3/2020
 * Creator: Nate Smith
 * 
 * Description: The MoveHandDown class that uses Raycasts.
 * Moves the current GameObject down according to the distance from the object to the surface.
 * Intended for rotational bodies.
 */
public class MoveHandDownRaycast : MoveHandDown
{
    // Public Variables.
    public float minHeightAdjusted = 1.6f;
    public float maxHeightAdjusted = 1.8f;
    public RaycastHit raycastHit;

    // Private Variables.
    private DrawLine drawLine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetHeight = maxHeight;
        drawLine = GetComponentInChildren<DrawLine>();
    }

    void FixedUpdate()
    {
        Ray curHeight = new Ray(transform.position, transform.up * -transform.localPosition.y);
        Debug.DrawRay(transform.position, transform.up * -transform.localPosition.y, Color.blue);

        // Adjust the raised and lowered heights of the machine based off the distance from the body being rotated around.
        if (Physics.Raycast(transform.position, -transform.up, out raycastHit, transform.localPosition.y, 1<<0, QueryTriggerInteraction.Ignore))
        {
            minHeightAdjusted = transform.localPosition.y - raycastHit.distance + minHeight - drawLine.transform.localPosition.y;
            maxHeightAdjusted = transform.localPosition.y - raycastHit.distance + maxHeight - drawLine.transform.localPosition.y;
        }

        // While the mouse is clicked, and the game is not paused, move toward lowered position
        if (!GameManager.instance.levelTransition && Input.GetMouseButton(0) && !GameManager.instance.screenshotText.enabled)
        {
            down = true;
            targetHeight = minHeightAdjusted;
        }
        // Otherwise, while the mouse is not clicked, and the game is not paused, move toward raised position
        else
        {
            down = false;
            targetHeight = maxHeightAdjusted;
        }

        // Locally linearly interpolate toward the intended height.
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, targetHeight, 0), Time.fixedDeltaTime * posSpeed);
    }
}
