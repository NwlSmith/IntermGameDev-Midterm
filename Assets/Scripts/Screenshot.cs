using UnityEngine;
using System.Collections;
/*
* Date created: 7/21/2010
* Creator: jashan
* 
* Description: The MoveHandDown class that uses Raycasts.
* Moves the current GameObject down according to the distance from the object to the surface.
* Intended for rotational bodies.
* 
* Based on code from a tutorial by Unity forum member Jashan:
* https://answers.unity.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
*/
public class Screenshot : MonoBehaviour
{
    // Public Variables.
    public int resWidth = 2550;
    public int resHeight = 3300;
    public bool takeHiResShot = false;

    /*
     * Generate the name for this screenshot
     * Invoked in LateUpdate().
     */
    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    /*
     * Generate the name for this screenshot
     * Invoked in Update() in GameManager.cs.
     */
    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    private void LateUpdate()
    {
        if (takeHiResShot)
        {
            // Retrieve texture from camera.
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            // Encode texture onto image.
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));

            // Clean up.
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            takeHiResShot = false;
        }
    }
}