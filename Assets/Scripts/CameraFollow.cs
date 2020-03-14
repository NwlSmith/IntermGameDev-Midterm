using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/21/2020
 * Creator: Nate Smith
 * 
 * Description: The Camera Follow Class.
 * Controls movement of the camera,
 * following the tattoo machine, zooming in and out, and displaying final tattoo.
 */
public class CameraFollow : MonoBehaviour
{
    // Public Variables.
    public GameObject targetGO;
    public float posSpeed = 10.0f;
    public float rotSpeed = 10.0f;
    public float curZoomDistance;
    public float minZoomDistance = .6f;
    public float maxZoomDistance = 2.5f;
    public float zoomSpeed = 100f;

    // Private Variables.
    private Vector3 posOffset;
    private Vector3 rotOffset;

    void Start()
    {
        // Retrieve starting positional data.
        posOffset = targetGO.transform.position - transform.position;
        rotOffset = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        // Calculate zoom distance.
        curZoomDistance = Vector3.Distance(targetGO.transform.position, transform.position);
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 change = transform.forward * -scroll * zoomSpeed * Time.deltaTime;

        // Adjust offset according to zoom distance, if distance is within acceptable range.
        if (Vector3.Magnitude(posOffset + change) < maxZoomDistance && Vector3.Magnitude(posOffset + change) > minZoomDistance)
            posOffset += change;
    }

    void FixedUpdate()
    {
        // Change the position and rotation of the camera to follow a GameObject, taking into accound the position and rotational offset.
        transform.position = Vector3.Slerp(transform.position, targetGO.transform.position - posOffset, Time.fixedDeltaTime * posSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotOffset), Time.fixedDeltaTime * rotSpeed);
    }
}
