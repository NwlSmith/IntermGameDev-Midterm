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
        exitButton2.GetComponent<Image>().raycastTarget = false;
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
        pauseImg.GetComponent<Animator>().SetTrigger("FullWhite");
        yield return new WaitForSeconds(whiteFadeDuration);

        GameManager.instance.gradientImg.GetComponent<Animator>().SetTrigger("Trans");

        // Update Stencils.
        stencils[curStencil].gameObject.SetActive(false);
        curStencil++;
        // If that was NOT the last stencil, enable the next one.
        if (curStencil < stencils.Length)
        {
            AudioManager.instance.TransitionTrack(curStencil + 2);
            stencils[curStencil].gameObject.SetActive(true);
            GameManager.instance.curStencil = stencils[curStencil];
            gm.progressText.GetComponent<Animator>().SetTrigger("Black");
            gm.UpdateProgressText(1, 1);
            StartCoroutine(gm.RestartGame());
            // hide screenshot and next buttons
        }
        // If that was the last stencil, end the game.
        else
        {
            gm.nextButton.GetComponent<Animator>().SetTrigger("Trans");
            gm.nextButton.GetComponent<Image>().raycastTarget = false;
            StartCoroutine(gm.LerpCameraToPos(finishedCameraPos));
            // Display screenshot and Endgame buttons
            exitButton2.GetComponent<Animator>().SetTrigger("Black");
            exitButton2.GetComponent<Image>().raycastTarget = true;
            gm.screenshotButton.GetComponent<Animator>().SetTrigger("Black");
            gm.screenshotButton.GetComponent<Image>().raycastTarget = true;
            gm.gradientImg.GetComponent<Animator>().SetTrigger("FullWhite");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Fade to transparent.
        pauseImg.GetComponent<Animator>().SetTrigger("Trans");
    }


}
