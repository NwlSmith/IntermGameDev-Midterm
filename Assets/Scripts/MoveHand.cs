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

    private float mouseX;
    private float mouseY;
    private float height = .5f;

    private Vector3 posOffset;
    private Vector3 targetPos;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //posOffset = targetGO.transform.position - transform.position;

        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
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
        

        targetPos += new Vector3(mouseX * sensitivityX, 0, mouseY * sensitivityY);
        targetPos = new Vector3(targetPos.x, height, targetPos.z);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(Vector3.Slerp(transform.position, targetPos, Time.fixedDeltaTime * posSpeed));
    }
}
