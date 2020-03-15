using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
 * Date created: 3/14/2020
 * Creator: Nate Smith
 * 
 * Description: The class which manages the intro sequence.
 */
public class IntroSequence : MonoBehaviour
{
    // Public variables.
    public float duration = 2f;
    public Text titleText;
    public Text nameText;
    public Text buttonText;
    public Image buttonImg;
    public Image fadeImg;
    public Vector3[] rotationTargets;
    public Color transparent = new Color(0f, 0f, 0f, 0f);
    public Color fullColor = new Color(0f, 0f, 0f, 1f);
    public int curRotTarget = 1;

    void Start()
    {
        StartCoroutine(Sequence());
        titleText.color = transparent;
        nameText.color = transparent;
        buttonText.color = transparent;
        buttonImg.color = transparent;
        fadeImg.color = fullColor;
    }

    private IEnumerator Sequence()
    {
        GameObject cameraGO = Camera.main.gameObject;

        // Fade in from white.
        float elapsedTime = 0f;
        Color startColor = fadeImg.color;
        Color targetColor = new Color(1f, 1f, 1f, 0f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeImg.color = Color.Lerp(startColor, targetColor, (elapsedTime / duration));
            yield return null;
        }
        fadeImg.color = targetColor;

        foreach (Vector3 targetRot in rotationTargets)
        {
            // Rotate to position.
            elapsedTime = 0f;
            Quaternion startRot = cameraGO.transform.rotation;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                cameraGO.transform.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(targetRot), (elapsedTime / duration));
                yield return null;
            }
            cameraGO.transform.rotation = Quaternion.Euler(targetRot);

            yield return new WaitForSeconds(.5f);
        }

        // Fade to white.
        elapsedTime = 0f;
        startColor = fadeImg.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeImg.color = Color.Lerp(startColor, Color.white, (elapsedTime / duration));
            yield return null;
        }
        fadeImg.color = Color.white;

        // Fade Ink in.
        elapsedTime = 0f;
        startColor = titleText.color;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            titleText.color = Color.Lerp(startColor, fullColor, (elapsedTime / (duration / 2)));
            yield return null;
        }
        titleText.color = fullColor;

        // Fade name in.
        elapsedTime = 0f;
        startColor = nameText.color;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            nameText.color = Color.Lerp(startColor, fullColor, (elapsedTime / (duration / 2)));
            yield return null;
        }
        nameText.color = fullColor;

        // Fade buttons in.
        elapsedTime = 0f;
        startColor = buttonText.color;
        Color startColorButton = buttonImg.color;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / (duration / 2);
            buttonText.color = Color.Lerp(startColor, fullColor, t);
            buttonImg.color = Color.Lerp(startColorButton, fullColor, t);
            yield return null;
        }
        buttonText.color = fullColor;
        buttonImg.color = fullColor;
    }

    /*
     * Starts the game.
     * Calls StartGameCO() coroutine. Fades to white, fades out buttons, and loads next scene.
     * Called by button on IntroSequence.
     */
    public void StartGame()
    {
        Debug.Log("Clicked");
        StartCoroutine(StartGameCO());
    }

    private IEnumerator StartGameCO()
    {
        // Fade to white and fade out buttons.
        float elapsedTime = 0f;
        // Fade button out.
        elapsedTime = 0f;
        Color startColor = buttonText.color;
        Color startColorButton = buttonImg.color;
        while (elapsedTime < duration / 4)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / (duration / 4);
            buttonText.color = Color.Lerp(startColor, transparent, t);
            buttonImg.color = Color.Lerp(startColorButton, transparent, t);
            yield return null;
        }
        buttonText.color = transparent;
        buttonImg.color = transparent;

        // Fade name out.
        elapsedTime = 0f;
        startColor = nameText.color;
        while (elapsedTime < duration / 4)
        {
            elapsedTime += Time.unscaledDeltaTime;
            nameText.color = Color.Lerp(startColor, transparent, (elapsedTime / (duration / 4)));
            yield return null;
        }
        nameText.color = transparent;

        // Fade title out.
        elapsedTime = 0f;
        startColor = titleText.color;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            titleText.color = Color.Lerp(startColor, transparent, (elapsedTime / (duration / 2)));
            yield return null;
        }
        titleText.color = transparent;

        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
}
