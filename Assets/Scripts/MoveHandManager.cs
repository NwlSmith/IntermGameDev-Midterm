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

    public float transitionDuration = .5f;

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

        // Save current position
        initPosGunRotator = moveHandRotate.transform.position;
        initRotGunRotator = moveHandRotate.transform.rotation;

        // Deactivate unused moveHand scripts
        if (moveHandDown)
            moveHandDown.enabled = false;
        if (moveHandRotate)
            moveHandRotate.enabled = false;

        // Smoothly transition to Flat positions
        StartCoroutine(Transition());
    }

    private void InitRound()
    {
        round = true;

        // Store current position
        initPosGunHolder = moveHand.transform.position;

        // Deactivate unused moveHand script
        if (moveHand)
            moveHand.enabled = false;

        // Smoothly transition to Round positions
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        // Smoothly lerp to the target.
        float elapsedTime = 0f;
        // Start position for GunHolder
        Vector3 startPosGH = moveHand.transform.position;
        // Start position and rotation for GunRotator
        Vector3 startPosGR = moveHandRotate.transform.position;
        Quaternion startRotGR = moveHandRotate.transform.rotation;

        // Spherically interpolate GunHolder, and GunRotator to initial positions + rotations.
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float timeStep = elapsedTime / transitionDuration;
            moveHand.transform.position = Vector3.Slerp(startPosGH, initPosGunHolder, timeStep);
            moveHandRotate.transform.position = Vector3.Slerp(startPosGR, initPosGunRotator, timeStep);
            moveHandRotate.transform.rotation = Quaternion.Slerp(startRotGR, initRotGunRotator, timeStep);
            yield return null;
        }
        moveHand.transform.position = initPosGunHolder;
        moveHandRotate.transform.position = initPosGunRotator;
        moveHandRotate.transform.rotation = initRotGunRotator;


        if (round)
        {
            // Activate moveHand scripts
            if (moveHandDown)
                moveHandDown.enabled = true;
            if (moveHandRotate)
                moveHandRotate.enabled = true;
        }
        else
        {
            // Activate MoveHand script
            if (moveHand)
                moveHand.enabled = true;
            moveHandRotate.transform.rotation = Quaternion.identity;
        }

    }

}
