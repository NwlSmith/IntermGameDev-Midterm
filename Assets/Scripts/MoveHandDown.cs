using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandDown : MonoBehaviour
{
    public float targetHeight;
    public float minHeight = 1.6f;
    public float maxHeight = 1.7f;
    public bool down = false;
    public float posSpeed = 10f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetHeight = maxHeight;
        
    }

    // Update is called once per frame
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
        
        // Moveposition is not working correctly
        //Vector3 moveTo = Vector3.Lerp(transform.localPosition, new Vector3(0, targetHeight - minHeight, 0), Time.fixedDeltaTime * posSpeed);
        //Debug.Log("moveTo = " + moveTo + " moveTo Transformed: "+ transform.TransformPoint(moveTo));
        //rb.MovePosition(moveTo);
    }
}
