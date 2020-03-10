using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Material curInk;
    public float curWidth = .05f;
    public float bufferVal;
    public GameObject bloodParticle;
    public float bloodLikelyhood = 10f;
    public GameObject line;
    public float numLines = 0;
    public GameObject lineHolder;
    public MeshRenderer[] needleMeshes;

    protected GameObject curLine;
    private LineRenderer lR;
    private List<Vector3> needlePos = new List<Vector3>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Skin")
        {
            CreateLine();
            numLines++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Skin")
        {
            Vector3 tempNeedlePos = LinePosition();
            if (Vector3.Distance(tempNeedlePos, needlePos[needlePos.Count - 1]) > bufferVal)
            {
                UpdateLine(tempNeedlePos);
            }
        }
    }

    protected void CreateLine()
    {
        curLine = Instantiate(line, Vector3.zero, LineNormal());
        curLine.transform.parent = lineHolder.transform;
        lR = curLine.GetComponent<LineRenderer>();
        needlePos.Clear();

        lR.material = curInk;
        lR.startWidth = curWidth;
        //lR.endWidth = curWidth;

        // replace this with needle position
        needlePos.Add(LinePosition());
        needlePos.Add(LinePosition());

        lR.SetPosition(0, needlePos[0]);
        lR.SetPosition(1, needlePos[1]);
    }

    protected virtual void UpdateLine(Vector3 newNeedlePos)
    {
        needlePos.Add(newNeedlePos);
        lR.positionCount++;
        lR.SetPosition(lR.positionCount - 1, newNeedlePos);

        GenerateBlood(newNeedlePos);
    }

    protected virtual Vector3 LinePosition()
    {
        return new Vector3(transform.position.x, -.59f + numLines / 10000, transform.position.z);
    }

    protected virtual Quaternion LineNormal()
    {
        return Quaternion.Euler(90, 0, 0);
    }

    protected virtual Quaternion BloodNormal()
    {
        return Quaternion.identity;
    }

    protected virtual Vector3 BloodPos()
    {
        return LinePosition();
    }

    private void GenerateBlood(Vector3 newNeedlePos)
    {
        if (Random.Range(0, bloodLikelyhood) <= 1)
        {
            Instantiate(bloodParticle, BloodPos(), BloodNormal());
            AudioManager.instance.BloodSound();
        }
    }


}
