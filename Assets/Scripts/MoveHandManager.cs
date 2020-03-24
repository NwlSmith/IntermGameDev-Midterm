using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/27/2020
 * Creator: Nate Smith
 * 
 * Description: The MoveHand Manager Class.
 * Is a single instance static object - There should only be 1 MoveHandManager.
 * The transitions between horizontal and rotational movement.
 */
public class MoveHandManager : MonoBehaviour
{
    // Static instance of the object.
    public static MoveHandManager instance = null;

    // Public Variables.
    public bool round = true;
    public float transitionDuration = .2f;
    public MoveHand moveHand;
    public MoveHandDown moveHandDown;
    public MoveHandRotate moveHandRotate;

    // Position and rotation of the MoveHand elements when transitioning to horizontal movement.
    public Vector3 flatInitPosGunHolder = new Vector3(-2.3171f, 2.1f, -2f);
    public Vector3 flatInitPosGunRotator = Vector3.zero;
    public Quaternion flatInitRotGunRotator = Quaternion.identity;

    // Position and rotation of the MoveHand elements when when transitioning to rotational movement.
    public Vector3 roundInitPosGunHolder = new Vector3(0f, 1.605947f, -0f);
    public Vector3 roundInitPosGunRotator = new Vector3(-2.92f, 0, 0);
    public Quaternion roundInitRotGunRotator = Quaternion.Euler(new Vector3(-40, 0, 0));

    // Target position and rotation of the MoveHand elements.
    public Vector3 targetPosGunHolder;
    public Vector3 targetPosGunRotator;
    public Quaternion targetRotGunRotator;

    private void Start()
    {
        // Ensure that there is only one instance of the MoveHandManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /*
     * Transition to horizontal movement.
     * Called in Update() in MoveHandRotation.
     */
    public void InitFlat()
    {
        round = false;

        // Set target orientations.
        targetPosGunHolder = flatInitPosGunHolder;
        targetPosGunRotator = flatInitPosGunRotator;
        targetRotGunRotator = flatInitRotGunRotator;

        // Deactivate unused moveHand scripts.
        if (moveHandDown)
            moveHandDown.enabled = false;
        if (moveHandRotate)
            moveHandRotate.enabled = false;

        // Smoothly transition to Flat positions.
        StartCoroutine(Transition());
    }

    /*
     * Transition to rotational movement.
     * Called in Update() in MoveHand.
     */
    public void InitRound()
    {
        round = true;

        // Set target orientations.
        targetPosGunHolder = roundInitPosGunHolder;
        targetPosGunRotator = roundInitPosGunRotator;
        targetRotGunRotator = roundInitRotGunRotator;

        // Deactivate unused moveHand script.
        if (moveHand)
            moveHand.enabled = false;

        // Smoothly transition to Round positions.
        StartCoroutine(Transition());
    }

    /*
     * Move elements to target positions.
     * Called in InitRound() and InitFlat().
     */
    private IEnumerator Transition()
    {
        // Smoothly lerp to the target.
        float elapsedTime = 0f;
        // Start position for GunHolder.
        Vector3 startPosGH = moveHand.transform.localPosition;
        // Start position and rotation for GunRotator.
        Vector3 startPosGR = moveHandRotate.transform.localPosition;
        Quaternion startRotGR = moveHandRotate.transform.rotation;

        // Spherically interpolate GunHolder, and GunRotator to target positions + rotations.
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float timeStep = elapsedTime / transitionDuration;
            moveHand.transform.localPosition = Vector3.Slerp(startPosGH, targetPosGunHolder, timeStep);
            moveHandRotate.transform.localPosition = Vector3.Slerp(startPosGR, targetPosGunRotator, timeStep);
            moveHandRotate.transform.rotation = Quaternion.Slerp(startRotGR, targetRotGunRotator, timeStep);
            yield return null;
        }
        // Set GunHolder, and GunRotator to target positions + rotations.
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
                moveHandRotate.targetPos = targetPosGunRotator + moveHandRotate.transform.parent.transform.position;
                moveHandRotate.targetRot = new Vector3(-50, 0, 0);
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