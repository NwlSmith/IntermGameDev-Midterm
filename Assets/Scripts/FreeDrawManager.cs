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
        finishedText.GetComponent<Animator>().SetTrigger("Black");
        screenshotButton.GetComponent<Image>().raycastTarget = false;
        exitButton.GetComponent<Image>().raycastTarget = false;
        unpauseButton.GetComponent<Image>().raycastTarget = false;
    }

    /*
     * Disable and enable UI, enable cursor, deactivate the tattoo hand.
     * Called in Update(), or from a button press.
     */
    protected override void FinishTheDrawing()
    {
        finishedText.GetComponent<Animator>().SetTrigger("Trans");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        moveHandRotate.gameObject.SetActive(false);
        finished = true;
        StartCoroutine(LerpCameraToPos(nextCameraTarget));
        gradientImg.GetComponent<Animator>().SetTrigger("FullWhite");
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
