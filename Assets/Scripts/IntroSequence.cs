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
    public Text freeText;
    public Image freeImg;
    public Text exitText;
    public Image exitImg;
    public Text websiteText;
    public Image websiteImg;
    public Image fadeImg;
    public Vector3[] rotationTargets;
    public Color transparent = new Color(0f, 0f, 0f, 0f);
    public Color fullColor = new Color(0f, 0f, 0f, 1f);
    public int curRotTarget = 1;
    public bool justTitle = false;

    void Start()
    {
        if (justTitle)
        {
            fadeImg.color = Color.white;
            StartCoroutine(TextSequence());
        }
        else
        {
            fadeImg.color = fullColor;
            StartCoroutine(CameraSequence());
        }
    }

    /*
     * Intro camera sequence.
     * Called in Start().
     */
    private IEnumerator CameraSequence()
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
        fadeImg.GetComponent<Animator>().SetTrigger("FullWhite");
        elapsedTime = 0f;
        startColor = fadeImg.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeImg.color = Color.Lerp(startColor, Color.white, (elapsedTime / duration));
            yield return null;
        }
        fadeImg.color = Color.white;

        StartCoroutine(TextSequence());
    }

    private IEnumerator TextSequence()
    {
        // Fade Ink in.
        titleText.GetComponent<Animator>().SetTrigger("Black");
        yield return new WaitForSeconds(.75f);

        // Fade name in.
        nameText.GetComponent<Animator>().SetTrigger("Black");
        yield return new WaitForSeconds(.75f);

        // Fade begin button in.
        buttonText.GetComponent<Animator>().SetTrigger("Black");
        buttonImg.GetComponent<Animator>().SetTrigger("Black");
        yield return new WaitForSeconds(.75f);

        // Fade rest of buttons in.
        freeText.GetComponent<Animator>().SetTrigger("Black");
        freeImg.GetComponent<Animator>().SetTrigger("Black");
        exitText.GetComponent<Animator>().SetTrigger("Black");
        exitImg.GetComponent<Animator>().SetTrigger("Black");
        websiteText.GetComponent<Animator>().SetTrigger("Black");
        websiteImg.GetComponent<Animator>().SetTrigger("Black");
    }
    /*
     * Intro text sequence.
     * Called in Start() and CameraSequence().
     *//*
    private IEnumerator TextSequence()
    {
        fadeImg.color = Color.white;
        // Fade Ink in.
        float elapsedTime = 0f;
        Color startColor = titleText.color;
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

        // Fade begin button in.
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

        // Fade rest of buttons in.
        elapsedTime = 0f;
        startColor = freeText.color;
        startColorButton = freeImg.color;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / (duration / 2);
            freeText.color = Color.Lerp(startColor, fullColor, t);
            freeImg.color = Color.Lerp(startColorButton, fullColor, t);
            exitText.color = Color.Lerp(startColor, fullColor, t);
            exitImg.color = Color.Lerp(startColorButton, fullColor, t);
            websiteText.color = Color.Lerp(startColor, fullColor, t);
            websiteImg.color = Color.Lerp(startColorButton, fullColor, t);
            yield return null;
        }
        freeText.color = fullColor;
        freeImg.color = fullColor;
        exitText.color = fullColor;
        exitImg.color = fullColor;
        websiteText.color = fullColor;
        websiteImg.color = fullColor;
    }*/

    /*
     * Starts the game.
     * Calls StartGameCO() coroutine. Fades to white, fades out buttons, and loads next scene.
     * Called by button on IntroSequence.
     */
    public void StartGame()
    {
        AudioManager.instance.TransitionTrack();
        StartCoroutine(StartGameCO("Level 1"));
    }
    
    /*
     * Starts free hand mode.
     * Calls StartGameCO() coroutine. Fades to white, fades out buttons, and loads free hand scene.
     * Called by button on IntroSequence.
     */
    public void StartFreeHand()
    {
        AudioManager.instance.TransitionTrack(4);
        StartCoroutine(StartGameCO("Free Draw"));
    }

    /*
     * Coroutine that initiates the game.
     * Fades out buttons and loads next scene.
     * Called by StartGame().
     */
    private IEnumerator StartGameCO(string levelString)
    {
        // Fade rest of buttons out.
        freeText.GetComponent<Animator>().SetTrigger("Trans");
        freeImg.GetComponent<Animator>().SetTrigger("Trans");
        exitText.GetComponent<Animator>().SetTrigger("Trans");
        exitImg.GetComponent<Animator>().SetTrigger("Trans");
        websiteText.GetComponent<Animator>().SetTrigger("Trans");
        websiteImg.GetComponent<Animator>().SetTrigger("Trans");
        yield return new WaitForSeconds(.75f);

        // Fade begin button out.
        buttonText.GetComponent<Animator>().SetTrigger("Trans");
        buttonImg.GetComponent<Animator>().SetTrigger("Trans");

        // Fade name out.
        nameText.GetComponent<Animator>().SetTrigger("Trans");
        yield return new WaitForSeconds(.75f);

        // Fade Ink out.
        titleText.GetComponent<Animator>().SetTrigger("Trans");
        yield return new WaitForSeconds(.75f);
        

        SceneManager.LoadScene(levelString, LoadSceneMode.Single);
    }

    /*
     * Exits the game.
     * Called by button on IntroSequence.
     */
    public void EndGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    /*
     * Forwards the user to portfolio website.
     * Called by button on IntroSequence.
     */
    public void Website()
    {
        Application.OpenURL("http://www.nwlsmith.com");
    }
}
