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

            float x = targetPos.x + mouseX * sensitivityX;

            if (x > boundary.z)
                x = boundary.z;
            if (x < boundary.w)
                x = boundary.w;

            targetPos = new Vector3(x, targetPos.y, targetPos.z);

            float r = targetRot.x + mouseY * sensitivityY;

            if (r > boundary.x)
                r = boundary.x;
            if (r < boundary.y)
                r = boundary.y;

            targetRot = new Vector3(r, 0, 0);
        }
    }

    protected override void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(targetRot), Time.fixedDeltaTime * rotSpeed));
    }
}
