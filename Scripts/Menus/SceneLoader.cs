using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator Transition;
    public float transitionTime;

    public void LoadNextScene(string nextScene)
    {
        StartCoroutine(LoadScene(nextScene));
    }

    IEnumerator LoadScene(string sceneName)
    {
        // play animation
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
