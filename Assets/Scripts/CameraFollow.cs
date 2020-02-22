using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject targetGO;
    public float posSpeed = 10.0f;
    public float rotSpeed = 10.0f;
    private Vector3 posOffset;
    private Vector3 rotOffset;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = targetGO.transform.position - transform.position;
        //rotOffset = targetGO.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, targetGO.transform.position - posOffset, Time.fixedDeltaTime * posSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetGO.transform.rotation, Time.fixedDeltaTime * rotSpeed);
    }
}
