using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineRotation : DrawLine
{

    protected override void UpdateLine(Vector3 newNeedlePos)
    {
        base.UpdateLine(newNeedlePos);
        CreateLine();
    }

    protected override Vector3 LinePosition()
    {
        return transform.position;
    }

    protected override Quaternion LineNormal()
    {
        return Quaternion.Euler(transform.parent.parent.rotation.eulerAngles.x + 90, 0, 0);
    }

    protected override Quaternion BloodNormal()
    {
        return Quaternion.Euler(transform.parent.parent.rotation.eulerAngles.x, 0, 0);
    }

    protected override Vector3 BloodPos()
    {
        return new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
    }
}
