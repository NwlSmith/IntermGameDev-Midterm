﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandDownRaycast : MoveHandDown
{

    public float minHeightAdjusted = 1.6f;
    public float maxHeightAdjusted = 1.8f;
    public Vector3 rayCastNormal;
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

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, transform.localPosition.y, 1<<0, QueryTriggerInteraction.Ignore))
        {
            rayCastNormal = hit.normal;
            minHeightAdjusted = transform.localPosition.y - hit.distance + minHeight - drawLine.transform.localPosition.y;
            maxHeightAdjusted = transform.localPosition.y - hit.distance + maxHeight - drawLine.transform.localPosition.y;
        }

        // if mouse down, move toward center
        if (Input.GetMouseButton(0))
        {
            down = true;
            targetHeight = minHeightAdjusted;
        }
        else
        {
            down = false;
            targetHeight = maxHeightAdjusted;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, targetHeight, 0), Time.fixedDeltaTime * posSpeed);
    }
}