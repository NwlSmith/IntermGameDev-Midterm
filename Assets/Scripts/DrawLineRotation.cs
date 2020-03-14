using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The Draw Line class for rotational spaces.
 * Draws line of ink of the chosen width and color.
 * Draws on curved surfaces.
 */
public class DrawLineRotation : DrawLine
{
    // The raycast script that calculates distance to the skin.
    private MoveHandDownRaycast moveHandDownRaycast;

    private void Start()
    {
        moveHandDownRaycast = GetComponentInParent<MoveHandDownRaycast>();
    }

    /*
     * Update the current line and create a new one.
     * Update the current line to extend to the buffer distance, then create a new line to go around curves.
     * Called in OnTriggerEnter().
     */
    protected override void UpdateLine(Vector3 newNeedlePos)
    {
        base.UpdateLine(newNeedlePos);
        CreateLine();
    }

    /*
     * Calculate the position of the line.
     * Lines will be created slightly above the current surface along the raycast.
     * Called in CreateLine() and OnTriggerStay().
     */
    protected override Vector3 LinePosition()
    {
        return moveHandDownRaycast.raycastHit.point + moveHandDownRaycast.raycastHit.normal * .001f * (numLines % 10);
    }

    /*
     * Calculate the normal vector of the line.
     * Lines will face in the normal direction of the skin model.
     * Called in CreateLine().
     */
    protected override Quaternion LineNormal()
    {
        return Quaternion.FromToRotation(-Vector3.forward, moveHandDownRaycast.raycastHit.normal);
    }

    /*
     * Calculate the position vector of the blood particles.
     * Lines will be slightly above the InkProjector GameObject.
     * Called in GenerateBlood().
     */
    protected override Vector3 BloodPos()
    {
        return new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
    }

    /*
     * Calculate the normal vector of the blood particle.
     * Lines will face in the direction of the rotation of the MoveHandRotate GameObject.
     * Called in CreateLine().
     */
    protected override Quaternion BloodNormal()
    {
        return Quaternion.Euler(transform.parent.parent.rotation.eulerAngles.x, 0, transform.parent.parent.rotation.eulerAngles.z);
    }
}
