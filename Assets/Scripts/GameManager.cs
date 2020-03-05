using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool cursorLocked = true;

    public Text pauseText;
    public Image pauseImg;

    public float pauseLerpDuration = .75f;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseText.enabled = false;
        pauseImg.color = new Color(1f, 1f, 1f, 0f);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;
            if (cursorLocked)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                StartCoroutine(TimeLerp(1f));
                StartCoroutine(FadeLerp(new Color(1f, 1f, 1f, 0f)));
                pauseText.enabled = false;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(TimeLerp(0f));
                StartCoroutine(FadeLerp(new Color(1f, 1f, 1f, .4f)));
                pauseText.enabled = true;
            }
        }
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
     * Lerp a white image to desired opacity.
     * The image is smoothly faded in based on:
     * The fraction of how much time has passed since the start of the Lerp
     * Divided by the total duration of the Lerp.
     * Invoked by Update.
     * targetTimeScale: the target timescale that will be achieved at the end of the lerp.
     */
    private IEnumerator FadeLerp(Color targetColor)
    {
        float duration = .75f;
        float elapsedTime = 0f;
        float startTimeScale = Time.timeScale;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(pauseImg.color, targetColor, (elapsedTime / duration));
            yield return null;
        }
        pauseImg.color = targetColor;
    }
}
