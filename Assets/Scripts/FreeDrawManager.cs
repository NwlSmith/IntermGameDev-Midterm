using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeDrawManager : GameManager
{

    /*
     * Deactivate all UI at the start of the scene.
     * Called in Start().
     */
    protected override void DeactivateUI()
    {
        // Disable the text UI and white fade UI.
        pauseText.enabled = false;
        solidImg.color = new Color(1f, 1f, 1f, 1f);
        gradientImg.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(SolidFadeLerp(new Color(1f, 1f, 1f, 0f), 1.5f));

        finishedText.enabled = true;
        screenshotButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        unpauseButton.gameObject.SetActive(false);
    }

    /*
     * Disable and enable UI, enable cursor, deactivate the tattoo hand.
     * Called in Update(), or from a button press.
     */
    protected override void FinishTheDrawing()
    {
        finishedText.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        moveHandRotate.gameObject.SetActive(false);
        StartCoroutine(LerpCameraToPos(nextCameraTarget));
        StartCoroutine(GradientFadeLerp(new Color(1f, 1f, 1f, 1f), .75f));
        AudioManager.instance.PlayFinishedSound();
    }

    /*
     * Reload the current scene.
     * Called from button press.
     */
    public void RestartDrawing()
    {
        SceneManager.LoadScene("Free Draw", LoadSceneMode.Single);
    }
}
