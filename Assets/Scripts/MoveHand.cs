using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/20/2020
 * Creator: Nate Smith
 * 
 * Description: The Move Hand Class.
 * Handles movement on the horizontal plane.
 */
public class MoveHand : MonoBehaviour
{

    // Public Variables.
    public float posSpeed = 10f;
    
    public float sensitivityX = .1f;
    public float sensitivityY = .1f;
    public bool down;

    public Vector4 boundary; // top, bottom, right, left
    public Vector3 targetPos;

    public float height = .5f;
    public float upHeight = .5f;
    public float downHeight = 0f;

    // Private Variables.
    protected float mouseX;
    protected float mouseY;
    protected Rigidbody rb;


    protected virtual void Start()
    {
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        // If the cursor is locked and the player has not finished their tattoo, allow movement.
        if (!GameManager.instance.levelTransition && GameManager.instance.cursorLocked && !GameManager.instance.screenshotButton.gameObject.activeSelf)
        {
            // Retrieve Input.
            mouseX = -Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");

            // Detect if the player has clicked and lower the tattoo machine.
            if (Input.GetMouseButtonDown(0))
            {
                down = true;
                height = downHeight;
            }
            // Detect if the player has let go of the mouse and raise the tattoo machine.
            else if (Input.GetMouseButtonUp(0))
            {
                down = false;
                height = upHeight;
            }

            // Calculate X movement.
            float x = targetPos.x + mouseX * sensitivityX;

            // Constrain within boundaries.
            if (x > boundary.z)
                x = boundary.z;
            if (x < boundary.w)
                x = boundary.w;

            // Calculate X movement.
            float z = targetPos.z + mouseY * sensitivityY;

            // Constrain within boundaries.
            if (z > boundary.x)
            {
                z = boundary.x;
                // If the player gets to the bottom of the screen, transition to the round movehand script.
                MoveHandManager.instance.InitRound();
            }
            if (z < boundary.y)
                z = boundary.y;

            targetPos = new Vector3(x, height, z);
        }
    }

    protected virtual void FixedUpdate()
    {
        // Move the hand to the target position.
        rb.MovePosition(Vector3.Slerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
    }
}
