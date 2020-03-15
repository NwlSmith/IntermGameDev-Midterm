using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
 * Date created: 3/14/2020
 * Creator: Nate Smith
 * 
 * Description: The class which manages the transition between level 1 and level 2.
 */
public class Level1To2 : MonoBehaviour
{
    // Public variables.
    public Image pauseImg;

    /*
     * Transition to level 2.
     * Called in Next() in GameManager.cs.
     */
    public void Level2()
    {
        Debug.Log("Level2()");
        GameManager.instance.levelTransition = true;
        pauseImg = GameManager.instance.pauseImg;
        StartCoroutine(Transition());
    }

    /*
     * Transition to level 2, fading the screen.
     * Called in Level2().
     */
    private IEnumerator Transition()
    {
        Debug.Log("transitioning");
        float duration = 1.25f;
        float elapsedTime = 0f;
        Color startColor = pauseImg.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseImg.color = Color.Lerp(startColor, Color.white, (elapsedTime / duration));
            yield return null;
        }
        pauseImg.color = Color.white;
        Debug.Log("transitioned");
        SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
    }
}
