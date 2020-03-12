using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool cursorLocked = true;

    public bool cameraTransition = false;

    public Text pauseText;
    public Image pauseImg;
    public Text progressText;
    public Text finishedText;
    public Text screenshotText;

    public float pauseLerpDuration = .75f;

    public GameObject nextCameraTarget;

    private MoveHandRotate moveHandRotate;

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

        finishedText.enabled = false;
        screenshotText.enabled = false;
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

        if (finishedText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                progressText.enabled = false;
                finishedText.enabled = false;
                Camera.main.GetComponent<CameraFollow>().enabled = false;
                StartCoroutine(LerpCameraToPos(nextCameraTarget));
            }
        }

        if (screenshotText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                screenshotText.enabled = false;
                GetComponent<Screenshot>().takeHiResShot = true;
                moveHandRotate = FindObjectOfType<MoveHandRotate>();
                moveHandRotate.gameObject.SetActive(false);
                StartCoroutine(RestartGame());
            }
        }
    }

    public void UpdateProgressText(int curCount, int totalCount)
    {
        progressText.text = " " + Mathf.Round((totalCount - curCount) * 100f / totalCount) + "% finished";
    }

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

    private IEnumerator LerpCameraToPos(GameObject cameraPos)
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
        screenshotText.enabled = true;
    }

    private IEnumerator RestartGame()
    {
        yield return null;
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        moveHandRotate.gameObject.SetActive(true);
    }
}
