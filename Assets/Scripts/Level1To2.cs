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
    /*
     * Transition to level 2.
     * Called in Next() in GameManager.cs.
     */
    public void Level2()
    {
        Debug.Log("Level2()");
        GameManager.instance.levelTransition = true;
        GameManager.instance.gradientImg.GetComponent<Animator>().SetTrigger("FullWhite");
        StartCoroutine(Transition());
        AudioManager.instance.TransitionTrack();
    }

    /*
     * Transition to level 2 after the screen fade.
     * Called in Level2().
     */
    private IEnumerator Transition()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
    }
}
