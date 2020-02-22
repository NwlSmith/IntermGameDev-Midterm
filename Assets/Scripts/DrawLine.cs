using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Material curInk;
    public float bufferVal;
    public GameObject bloodParticle;
    public float bloodLikelyhood = 10f;
    public GameObject line;
    private GameObject curLine;
    private LineRenderer lR;
    private List<Vector3> needlePos = new List<Vector3>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Skin")
        {
            CreateLine();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Skin")
        {
            Vector3 tempNeedlePos = transform.position;
            if (Vector3.Distance(tempNeedlePos, needlePos[needlePos.Count - 1]) > bufferVal)
            {
                UpdateLine(tempNeedlePos);
            }
        }
    }

    private void CreateLine()
    {
        curLine = Instantiate(line, Vector3.zero, Quaternion.Euler(90, 0, 0));
        lR = curLine.GetComponent<LineRenderer>();
        needlePos.Clear();

        lR.material = curInk;

        // replace this with needle position
        needlePos.Add(transform.position);
        needlePos.Add(transform.position);

        lR.SetPosition(0, needlePos[0]);
        lR.SetPosition(1, needlePos[1]);
    }

    private void UpdateLine(Vector3 newNeedlePos)
    {
        needlePos.Add(newNeedlePos);
        lR.positionCount++;
        lR.SetPosition(lR.positionCount - 1, newNeedlePos);

        if (Random.Range(0, bloodLikelyhood) <= 1)
            Instantiate(bloodParticle, transform.position, Quaternion.identity);
    }
}
