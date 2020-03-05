using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHand : MonoBehaviour
{

    //public GameObject targetGO;
    public float posSpeed = 10f;
    
    public float sensitivityX = .1f;
    public float sensitivityY = .1f;
    public bool down;

    public Vector4 boundary; // top, bottom, right, left
    public Vector3 targetPos;

    protected float mouseX;
    protected float mouseY;
    protected float height = .5f;

    protected Rigidbody rb;


    protected virtual void Start()
    {
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {

        if (GameManager.instance.cursorLocked)
        {
            mouseX = -Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");

            if (Input.GetMouseButtonDown(0))
            {
                down = true;
                height = 0f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                down = false;
                height = .5f;
            }

            float x = targetPos.x + mouseX * sensitivityX;

            if (x > boundary.z)
                x = boundary.z;
            if (x < boundary.w)
                x = boundary.w;

            float z = targetPos.z + mouseY * sensitivityY;

            if (z > boundary.x)
            {
                z = boundary.x;
                MoveHandManager.instance.InitRound();
            }
            if (z < boundary.y)
                z = boundary.y;

            targetPos = new Vector3(x, height, z);
        }
    }

    protected virtual void FixedUpdate()
    {
        rb.MovePosition(Vector3.Slerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
    }
}
