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

    void Start()
    {
        stencils[1].gameObject.SetActive(false);
        stencils[2].gameObject.SetActive(false);
    }

    /*
     * Transition the next stencil.
     * Fades to white, enable the 
     * Reenable the progress text
     * Called in Next() in GameManager.cs.
     */
    public IEnumerator NextStencil()
    {
        // fade to white, enable the next stencil, restartgame(), reenable progresstext.
        GameManager gm = GameManager.instance;

        // Fade to white.
        float elapsedTime = 0f;
        float startTimeScale = Time.timeScale;
        while (elapsedTime < whiteFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(pauseImg.color, Color.white, (elapsedTime / whiteFadeDuration));
            yield return null;
        }
        pauseImg.color = Color.white;

        // Update Stencils.
        stencils[curStencil].gameObject.SetActive(false);
        curStencil++;
        // If that was NOT the last stencil, enable the next one.
        if (curStencil < stencils.Length)
        {
            stencils[curStencil].gameObject.SetActive(true);
            gm.progressText.enabled = true;
            gm.UpdateProgressText(1, 1);
            StartCoroutine(gm.RestartGame());
        }
        // If that was the last stencil, end the game.
        else
        {
            // IMPLEMENT ENDGAME
        }

        // Fade to transparent.
        elapsedTime = 0f;
        startTimeScale = Time.timeScale;
        Color target = new Color(1f, 1f, 1f, 0f);
        while (elapsedTime < whiteFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(pauseImg.color, target, (elapsedTime / whiteFadeDuration));
            yield return null;
        }
        pauseImg.color = target;
    }
}
