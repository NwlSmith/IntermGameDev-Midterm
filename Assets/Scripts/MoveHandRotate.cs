using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The Move Hand Class for Rotational spaces.
 * Handles rotational movement.
 */
public class MoveHandRotate : MoveHand
{
    // Public Variables.
    public Vector3 targetRot;
    public float rotSpeed = 20f;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPos = rb.position;
        targetRot = rb.rotation.eulerAngles;
    }

    protected override void Update()
    {
        // Take input while the game is not paused or suspended.
        if (!GameManager.instance.levelTransition && GameManager.instance.cursorLocked && !GameManager.instance.finishedPressed && !GameManager.instance.paused)
        {
            // Collect input.
            mouseX = -Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");

            // Calculate horizontal movement.
            float x = targetPos.x + mouseX * sensitivityX;

            if (x > boundary.z)
                x = boundary.z;
            if (x < boundary.w)
                x = boundary.w;

            targetPos = new Vector3(x, targetPos.y, targetPos.z);

            // Calculate rotational movement.
            float r = targetRot.x + mouseY * sensitivityY;

            if (r > boundary.x)
                r = boundary.x;
            if (r < boundary.y)
            {
                r = boundary.y;
                // If the machine has rotated past a certain point, transition to horizontal-plane movement.
                MoveHandManager.instance.InitFlat();
            }

            targetRot = new Vector3(r, 0, 0);
        }
    }

    protected override void FixedUpdate()
    {
        // Move the hand to the target position and rotation.
        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(targetRot), Time.fixedDeltaTime * rotSpeed));
    }
}
