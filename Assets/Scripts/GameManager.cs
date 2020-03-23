using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * Date created: 2/25/2020
 * Creator: Nate Smith
 * 
 * Description: The GameManager class.
 * Is a single instance static object - There should only be 1 GameManager.
 * Handles pausing, text, camera targetting, and scene transitions.
 */
public class GameManager : MonoBehaviour
{
    // Static instance of the object.
    public static GameManager instance = null;

    // Public Variables.
    public bool cursorLocked = true;

    public bool levelTransition = false;

    public Text pauseText;
    public Image solidImg;
    public Image gradientImg;
    public Text progressText;
    public Text finishedText;
    public Button screenshotButton;
    public Button nextButton;
    public Button exitButton;
    public Button unpauseButton;

    public float pauseLerpDuration = .75f;

    public GameObject nextCameraTarget;

    public Stencil curStencil;

    public Level1To2 level1To2;
    public Level2Transitions level2Transitions;

    // Protected Variables.
    protected MoveHandRotate moveHandRotate;

    protected void Start()
    {
        // Ensure that there is only one instance of the GameManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Lock the cursor and make invisible.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Deactivate the UI
        DeactivateUI();
        
        moveHandRotate = FindObjectOfType<MoveHandRotate>();
    }

    /*
     * Deactivate all UI at the start of the scene.
     * Called in Start().
     */
    protected virtual void DeactivateUI()
    {
        // Disable the text UI and white fade UI.
        pauseText.enabled = false;
        solidImg.color = new Color(1f, 1f, 1f, 1f);
        gradientImg.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(SolidFadeLerp(new Color(1f, 1f, 1f, 0f), 1.5f));

        finishedText.enabled = false;
        screenshotButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        unpauseButton.gameObject.SetActive(false);
    }

    protected void Update()
    {
        // If the player presses the pause key, toggle the pause mode.
        if (!levelTransition && Input.GetKeyDown(KeyCode.Escape) && !screenshotButton.gameObject.activeSelf)
        {
            cursorLocked = !cursorLocked;
            // Lock the cursor, restart time, get rid of the pause UI.
            if (cursorLocked)
            {
                Unpause();
            }
            // Unlock the cursor, stop time, enable the pause UI.
            else
            {
                Pause();
            }
        }

        // If the finish game text prompt is enabled, check for the key press.
        if (!levelTransition && finishedText.enabled && !unpauseButton.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Disable the normal UI and send the Camera to the display tattoo position.
                FinishTheDrawing();
            }
        }
    }

    /*
     * Disable and enable UI, enable cursor, deactivate the tattoo hand and the stencil.
     * Called in Update(), or from a button press.
     */
    protected virtual void FinishTheDrawing()
    {
        progressText.enabled = false;
        finishedText.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        moveHandRotate.gameObject.SetActive(false);
        StartCoroutine(LerpCameraToPos(nextCameraTarget));
        StartCoroutine(GradientFadeLerp(new Color(1f, 1f, 1f, .4f), .75f));
        AudioManager.instance.PlayFinishedSound();
        curStencil.GetComponent<MeshRenderer>().enabled = false;
        curStencil.DeactivateChildren();
    }

    /*
     * Take a screenshot, then transition to the next level/section.
     * Called in Update(), or from a button press.
     */
    public void Screenshot()
    {
        // Disable the finishedText UI, take a screenshot, and restart the game.
        GetComponent<Screenshot>().takeHiResShot = true;
        // The last screenshot should quit the game.
        if (!level1To2 && (level2Transitions && level2Transitions.curStencil >= level2Transitions.stencils.Length))
            EndGame();
        else
            Next();
    }

    /*
     * Transition to the next level/section.
     * Called in Update(), or from a button press.
     */
    public void Next()
    {
        screenshotButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (!level1To2)
        {
            StartCoroutine(level2Transitions.NextStencil());
        }
        else
        {
            level1To2.Level2();
        }
    }

    /*
     * Unpauses the game.
     * Locks the cursor, fades out to normal, restarts time, and disables pause UI
     * Called by pressing in 'escape' Update() OR the Unpause button.
     */
    public void Unpause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
        StartCoroutine(TimeLerp(1f));
        StartCoroutine(SolidFadeLerp(new Color(1f, 1f, 1f, 0f), .75f));
        pauseText.enabled = false;
        exitButton.gameObject.SetActive(false);
        unpauseButton.gameObject.SetActive(false);
    }

    /*
     * Pauses the game.
     * unlocks the cursor, fades the screen to white, stops time, and enables pause UI
     * Called by pressing in 'escape' Update().
     */
    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cursorLocked = false;
        StartCoroutine(TimeLerp(0f));
        StartCoroutine(SolidFadeLerp(new Color(1f, 1f, 1f, .4f), .75f));
        pauseText.enabled = true;
        exitButton.gameObject.SetActive(true);
        unpauseButton.gameObject.SetActive(true);
    }

    /*
     * Returns to main menu.
     * Called by button on IntroSequence.
     */
    public void MainMenu()
    {
        AudioManager.instance.TransitionTrack(0);
        StartCoroutine(MainMenuFadeCO());
    }

    /*
     * Returns to main menu after fading screen to white.
     * Called by MainMenu() in GameManager.cs.
     */
    private IEnumerator MainMenuFadeCO()
    {
        StartCoroutine(SolidFadeLerp(Color.white, 1f));
        pauseText.enabled = false;
        finishedText.enabled = false;
        screenshotButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        unpauseButton.gameObject.SetActive(false);
        if (finishedText)
            finishedText.enabled = false;
        if (progressText)
            progressText.enabled = false;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        Debug.Log("Transitioning now");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    /*
     * Exits the game.
     * Called by button in main menu.
     */
    public void EndGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    /*
     * Update the ProgressText to current percentage
     * Called in RemoveStencilCollider() in Stencil.cs.
     * curCount: current number of active StencilColliders.
     * totalCount: total number of active and inactive StencilColliders.
     */
    public void UpdateProgressText(int curCount, int totalCount)
    {
        progressText.text = "   " + Mathf.Round((totalCount - curCount) * 100f / totalCount) + "% finished";
    }

    /*
     * Remove the finishedText and retarget the camera to the assigned position.
     * Called in RemoveStencilCollider() in Stencil.cs.
     * newCameraTarget: The Stencil's camera target transform the camera should Lerp to.
     */
    public void StencilFinished(GameObject newCameraTarget)
    {
        finishedText.enabled = true;
        nextCameraTarget = newCameraTarget;
    }


    /*
     * Lerp time to desired timescale.
     * The timescale is smoothly reset based on:
     * The fraction of how much time has passed since the start of the Lerp
     * Divided by the total duration of the Lerp.
     * Invoked by Update.
     * targetTimeScale: the target timescale that will be achieved at the end of the lerp.
     */
    private IEnumerator TimeLerp(float targetTimeScale)
    {
        float duration = .75f;
        float elapsedTime = 0f;
        float startTimeScale = Time.timeScale;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.SmoothStep(startTimeScale, targetTimeScale, (elapsedTime / duration));
            yield return null;
        }
        Time.timeScale = targetTimeScale;
    }

    /*
     * Lerp the solid white image to desired opacity.
     * The image is smoothly faded in based on:
     * The fraction of how much time has passed since the start of the Lerp
     * Divided by the total duration of the Lerp.
     * Invoked by Update.
     * targetTimeScale: the target timescale that will be achieved at the end of the lerp.
     */
    public IEnumerator SolidFadeLerp(Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            solidImg.color = Color.Lerp(solidImg.color, targetColor, (elapsedTime / duration));
            yield return null;
        }
        solidImg.color = targetColor;
    }

    /*
     * Lerp a white gradient image to desired opacity.
     * The image is smoothly faded in based on:
     * The fraction of how much time has passed since the start of the Lerp
     * Divided by the total duration of the Lerp.
     * Invoked by Update.
     * targetTimeScale: the target timescale that will be achieved at the end of the lerp.
     */
    public IEnumerator GradientFadeLerp(Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            gradientImg.color = Color.Lerp(gradientImg.color, targetColor, (elapsedTime / duration));
            yield return null;
        }
        gradientImg.color = targetColor;
    }

    /*
     * Lerp the camera to the given GameObject.
     * The image is smoothly moved in based on:
     * The fraction of how much time has passed since the start of the Lerp
     * Divided by the total duration of the Lerp.
     * Invoked by NextStencil() in Level2Transitions.cs and Update().
     * targetTimeScale: the target timescale that will be achieved at the end of the lerp.
     */
    public IEnumerator LerpCameraToPos(GameObject cameraPos)
    {
        float duration = .75f;
        float elapsedTime = 0f;
        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Camera.main.transform.position = Vector3.Slerp(startPos, cameraPos.transform.position, (elapsedTime / duration));
            Camera.main.transform.rotation = Quaternion.Slerp(startRot, cameraPos.transform.rotation, (elapsedTime / duration));
            yield return null;
        }
        Camera.main.transform.position = cameraPos.transform.position;
        Camera.main.transform.rotation = cameraPos.transform.rotation;

        //Screenshot and "next" buttons should appear only if this is not the end of the game.
        if (level1To2 || (level2Transitions && level2Transitions.curStencil < level2Transitions.stencils.Length))
        {
            screenshotButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
        else if (GetComponent<FreeDrawManager>())
        {
            screenshotButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
    }

    /*
     * Wait a frame then restart the game.
     * Called so that the screenshot will not be obstructed by the tattooing arm.
     * Called in Update().
     */
    public IEnumerator RestartGame()
    {
        yield return null;
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        moveHandRotate.gameObject.SetActive(true);
    }
}
