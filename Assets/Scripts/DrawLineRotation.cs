using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineRotation : DrawLine
{

    private MoveHandDownRaycast moveHandDownRaycast;

    private void Start()
    {
        moveHandDownRaycast = GetComponentInParent<MoveHandDownRaycast>();
    }

    protected override void UpdateLine(Vector3 newNeedlePos)
    {
        base.UpdateLine(newNeedlePos);
        CreateLine();
    }

    protected override Vector3 LinePosition()
    {
        return moveHandDownRaycast.raycastHit.point + moveHandDownRaycast.raycastHit.normal * .001f * (numLines % 10);
    }

    protected override Quaternion LineNormal()
    {
        return Quaternion.FromToRotation(-Vector3.forward, moveHandDownRaycast.raycastHit.normal);
    }

    protected override Quaternion BloodNormal()
    {
        return Quaternion.Euler(transform.parent.parent.rotation.eulerAngles.x, 0, transform.parent.parent.rotation.eulerAngles.z);
    }

    protected override Vector3 BloodPos()
    {
        return new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
    }
}
