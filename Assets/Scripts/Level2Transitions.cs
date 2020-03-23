using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Date created: 3/14/2020
 * Creator: Nate Smith
 * 
 * Description: The class which manages the transitions between stencils in level 2.
 */
public class Level2Transitions : MonoBehaviour
{
    // Public variables.
    public Stencil[] stencils; // [0] = Hatchet, [1] = Web, [3] = Guillotine.
    public Image pauseImg;
    public int curStencil = 0;
    public float whiteFadeDuration = 2f;
    public GameObject finishedCameraPos;
    public Button exitButton2;

    void Start()
    {
        stencils[1].gameObject.SetActive(false);
        stencils[2].gameObject.SetActive(false);
        GameManager.instance.curStencil = stencils[0];
        exitButton2.gameObject.SetActive(false);
    }

    /*
     * Transition the next stencil.
     * Fades to white, enables the next stencil, reenables the progress text, then fades the white out.
     * Called in Next() in GameManager.cs.
     */
    public IEnumerator NextStencil()
    {
        // fade to white, enable the next stencil, restartgame(), reenable progresstext.
        GameManager gm = GameManager.instance;

        // Fade to white.
        float elapsedTime = 0f;
        Color startColor = pauseImg.color;
        while (elapsedTime < whiteFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(startColor, Color.white, (elapsedTime / whiteFadeDuration));
            yield return null;
        }
        pauseImg.color = Color.white;

        GameManager.instance.gradientImg.color = new Color(1f, 1f, 1f, 0f);

        // Update Stencils.
        stencils[curStencil].gameObject.SetActive(false);
        curStencil++;
        // If that was NOT the last stencil, enable the next one.
        if (curStencil < stencils.Length)
        {
            AudioManager.instance.TransitionTrack(curStencil + 2);
            stencils[curStencil].gameObject.SetActive(true);
            GameManager.instance.curStencil = stencils[curStencil];
            gm.progressText.enabled = true;
            gm.UpdateProgressText(1, 1);
            StartCoroutine(gm.RestartGame());
            // hide screenshot and next buttons
        }
        // If that was the last stencil, end the game.
        else
        {
            gm.nextButton.gameObject.SetActive(false);
            StartCoroutine(gm.LerpCameraToPos(finishedCameraPos));
            // Display screenshot and Endgame buttons
            exitButton2.gameObject.SetActive(true);
            gm.screenshotButton.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Fade to transparent.
        elapsedTime = 0f;
        startColor = pauseImg.color;
        Color target = new Color(1f, 1f, 1f, 0f);
        while (elapsedTime < whiteFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(startColor, target, (elapsedTime / whiteFadeDuration));
            yield return null;
        }
        pauseImg.color = target;
    }


}
