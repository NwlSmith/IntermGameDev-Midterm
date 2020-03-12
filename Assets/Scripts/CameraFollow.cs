using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject targetGO;
    public float posSpeed = 10.0f;
    public float rotSpeed = 10.0f;
    public float curZoomDistance;
    public float minZoomDistance = .6f;
    public float maxZoomDistance = 2.5f;
    public float zoomSpeed = 100f;
    private Vector3 posOffset;
    private Vector3 rotOffset;

    void Start()
    {
        posOffset = targetGO.transform.position - transform.position;
        rotOffset = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        curZoomDistance = Vector3.Distance(targetGO.transform.position, transform.position);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 change = transform.forward * -scroll * zoomSpeed * Time.deltaTime;

        if (Vector3.Magnitude(posOffset + change) < maxZoomDistance && Vector3.Magnitude(posOffset + change) > minZoomDistance)
            posOffset += change;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, targetGO.transform.position - posOffset, Time.fixedDeltaTime * posSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotOffset), Time.fixedDeltaTime * rotSpeed);
    }
}
