using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandManager : MonoBehaviour
{
    public static MoveHandManager instance = null;

    public MoveHand moveHand;
    public MoveHandDown moveHandDown;
    public MoveHandRotate moveHandRotate;

    public Vector3 initPosGunHolder;
    public Vector3 initPosGunRotator;
    public Quaternion initRotGunRotator;

    public Vector3 targetPosGunHolder;
    public Vector3 targetPosGunRotator;
    public Quaternion targetRotGunRotator;

    public bool round = true;

    public float transitionDuration = .2f;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        initPosGunHolder = new Vector3(-2.3171f, 2.1f, -2f);
        initPosGunRotator = new Vector3(-2.92f, 0, 0);
        initRotGunRotator = Quaternion.Euler(new Vector3(-40, 0, 0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.instance.pauseText.enabled && !GameManager.instance.screenshotText.enabled)
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

    public void InitFlat()
    {
        round = false;

        // Set target orientations
        targetPosGunHolder = initPosGunHolder;
        targetPosGunRotator = Vector3.zero;
        targetRotGunRotator = Quaternion.identity;

        // Deactivate unused moveHand scripts
        if (moveHandDown)
            moveHandDown.enabled = false;
        if (moveHandRotate)
            moveHandRotate.enabled = false;

        // Smoothly transition to Flat positions
        StartCoroutine(Transition());
    }

    public void InitRound()
    {
        round = true;

        // Set target orientations
        targetPosGunHolder = new Vector3(0f, 1.605947f, -0f);
        targetPosGunRotator = initPosGunRotator;
        targetRotGunRotator = initRotGunRotator;

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
        Vector3 startPosGH = moveHand.transform.localPosition;
        // Start position and rotation for GunRotator
        Vector3 startPosGR = moveHandRotate.transform.localPosition;
        Quaternion startRotGR = moveHandRotate.transform.rotation;

        // Spherically interpolate GunHolder, and GunRotator to initial positions + rotations.
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float timeStep = elapsedTime / transitionDuration;
            moveHand.transform.localPosition = Vector3.Slerp(startPosGH, targetPosGunHolder, timeStep);
            moveHandRotate.transform.localPosition = Vector3.Slerp(startPosGR, targetPosGunRotator, timeStep);
            moveHandRotate.transform.rotation = Quaternion.Slerp(startRotGR, targetRotGunRotator, timeStep);
            yield return null;
        }
        moveHand.transform.localPosition = targetPosGunHolder;
        moveHandRotate.transform.localPosition = targetPosGunRotator;
        moveHandRotate.transform.rotation = targetRotGunRotator;


        if (round)
        {
            // Activate moveHand scripts
            if (moveHandDown)
                moveHandDown.enabled = true;
            if (moveHandRotate)
            {
                moveHandRotate.enabled = true;
                moveHandRotate.targetPos = targetPosGunRotator - new Vector3(0, 1.6f, 0);
                //moveHandRotate.targetRot = targetRotGunRotator.eulerAngles;
            }
        }
        else
        {
            // Activate MoveHand script
            if (moveHand)
            {
                moveHand.enabled = true;
                moveHand.targetPos = moveHand.transform.position;
            }
        }
    }
}