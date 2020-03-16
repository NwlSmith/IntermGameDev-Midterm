using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 2/20/2020
 * Creator: Nate Smith
 * 
 * Description: The Draw Line class.
 * Draws line of ink of the chosen width and color.
 * Works on flat surfaces.
 */
public class DrawLine : MonoBehaviour
{
    // Public Variables.
    public Material curInk;
    public float curWidth = .05f;
    public float bufferVal;
    public GameObject bloodParticle;
    public float bloodLikelyhood = 10f;
    public GameObject line;
    public float numLines = 0;
    public GameObject lineHolder;
    public MeshRenderer[] needleMeshes;

    // Protected Variable.
    protected GameObject curLine;

    // Private Variables.
    private LineRenderer lR;
    private List<Vector3> needlePos = new List<Vector3>();

    private void OnTriggerEnter(Collider other)
    {
        // If the InkProjector GameObject collider enters an object with the "Skin" tag, draw a line.
        if (other.gameObject.tag == "Skin")
        {
            CreateLine();
            numLines++;
        }

        AudioManager.instance.PlayMachineSound();
    }

    private void OnTriggerStay(Collider other)
    {
        // Check that the InkProjector GameObject is still colliding with the Skin. 
        if (other.gameObject.tag == "Skin")
        {
            // If the InkProjector has moved a distance more than the buffer value, add to the line.
            Vector3 tempNeedlePos = LinePosition();
            if (Vector3.Distance(tempNeedlePos, needlePos[needlePos.Count - 1]) > bufferVal)
            {
                UpdateLine(tempNeedlePos);
            }
        }
    }

    /*
     * Instantiate a new line.
     * Lines will be created at the position of the InkProjector facing the normal direction of the surface.
     * Called in OnTriggerEnter().
     */
    protected void CreateLine()
    {
        curLine = Instantiate(line, Vector3.zero, LineNormal());
        curLine.transform.parent = lineHolder.transform;
        lR = curLine.GetComponent<LineRenderer>();
        needlePos.Clear();

        lR.material = curInk;
        lR.startWidth = curWidth;

        // Two positions are added to the LineRenderer so that single dots can be rendered.
        needlePos.Add(LinePosition());
        needlePos.Add(LinePosition());

        lR.SetPosition(0, needlePos[0]);
        lR.SetPosition(1, needlePos[1]);
    }

    /*
     * Extend the current line, generate blood if necessary.
     * Called in OnTriggerStay().
     */
    protected virtual void UpdateLine(Vector3 newNeedlePos)
    {
        needlePos.Add(newNeedlePos);
        lR.positionCount++;
        lR.SetPosition(lR.positionCount - 1, newNeedlePos);

        GenerateBlood(newNeedlePos);
    }

    /*
     * Calculate the position of the line.
     * Lines will be created slightly above the current surface.
     * Called in CreateLine() and OnTriggerStay().
     */
    protected virtual Vector3 LinePosition()
    {
        return new Vector3(transform.position.x, -.59f + numLines / 10000, transform.position.z);
    }

    /*
     * Calculate the normal vector of the line.
     * Lines will face upwards in the Y-direction.
     * Called in CreateLine().
     */
    protected virtual Quaternion LineNormal()
    {
        return Quaternion.Euler(90, 0, 0);
    }

    /*
     * Randomly instantiate blood particles.
     * Called in GenerateBlood().
     */
    private void GenerateBlood(Vector3 newNeedlePos)
    {
        if (Random.Range(0, bloodLikelyhood) <= 1)
        {
            Instantiate(bloodParticle, BloodPos(), BloodNormal());
            AudioManager.instance.BloodSound();
        }
    }

    /*
     * Calculate the position vector of the blood particles.
     * Lines will be at the same position of the current line.
     * Called in GenerateBlood().
     */
    protected virtual Vector3 BloodPos()
    {
        return LinePosition();
    }

    /*
     * Calculate the normal vector of the blood particles.
     * Lines will face upwards in the Y-direction.
     * Called in GenerateBlood().
     */
    protected virtual Quaternion BloodNormal()
    {
        return Quaternion.identity;
    }
}
