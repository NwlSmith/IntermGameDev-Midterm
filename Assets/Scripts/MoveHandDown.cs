using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The Draw Line class for moving up and down in horizontal spaces.
 * DEPRECATED.
 */
public class MoveHandDown : MonoBehaviour
{
    public float targetHeight;
    public float minHeight = 1.6f; 
    public float maxHeight = 1.8f;
    public bool down = false;
    public float posSpeed = 10f;
    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetHeight = maxHeight;
    }

    void Update()
    {
        // if mouse down, move toward center
        if (Input.GetMouseButtonDown(0))
        {
            down = true;
            targetHeight = minHeight;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            down = false;
            targetHeight = maxHeight;
        }

    }

    void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, targetHeight, 0), Time.fixedDeltaTime * posSpeed);
    }
}
