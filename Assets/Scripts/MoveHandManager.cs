using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandManager : MonoBehaviour
{

    public MoveHand moveHand;
    public MoveHandDown moveHandDown;
    public MoveHandRotate moveHandRotate;

    public Vector3 initPosGunHolder;
    public Vector3 initPosGunRotator;
    public Quaternion initRotGunRotator;

    public bool round = true;

    private void Start()
    {
        initPosGunHolder = moveHand.transform.position;
        initPosGunRotator = moveHandRotate.transform.position;
        initRotGunRotator = moveHandRotate.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (round)
            {
                InitFlat();
            }
            else
            {
                InitRound();
            }
        }
    }

    private void InitFlat()
    {
        round = false;

        if (moveHandDown)
            moveHandDown.enabled = false;
        if (moveHandRotate)
            moveHandRotate.enabled = false;

        if (moveHand)
            moveHand.enabled = true;

        moveHand.transform.position = initPosGunHolder;
        moveHandRotate.transform.position = initPosGunRotator;
        moveHandRotate.transform.rotation = initRotGunRotator;

    }

    private void InitRound()
    {
        round = true;

        if (moveHandDown)
            moveHandDown.enabled = true;
        if (moveHandRotate)
            moveHandRotate.enabled = true;

        if (moveHand)
            moveHand.enabled = false;

        moveHand.transform.position = initPosGunHolder;
        moveHandRotate.transform.position = initPosGunRotator;
        moveHandRotate.transform.rotation = initRotGunRotator;
    }
}
