using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandRotate : MoveHand
{

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
        if (GameManager.instance.cursorLocked)
        {
            // Collect input
            mouseX = -Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");

            // target moves on x axis,
            // target rotates on x axis
            targetPos = new Vector3(targetPos.x + mouseX * sensitivityX, targetPos.y, targetPos.z);

            targetRot = new Vector3(targetRot.x + mouseY * sensitivityY, 0, 0);
        }
    }

    protected override void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(targetRot), Time.fixedDeltaTime * rotSpeed));
    }
}
