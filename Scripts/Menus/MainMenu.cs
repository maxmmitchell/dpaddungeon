using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;

    public void Play()
    {
        FindObjectOfType<AudioManager>().PlayButtonSFX();
        FindObjectOfType<SceneLoader>().LoadNextScene("Level");
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().PlayButtonSFX();
        Application.Quit();
    }

    public void PlayButtonSFX()
    {
        AudioManager.instance.PlayButtonSFX();
    }

    public void RandomButton()
    {
        AudioManager.instance.Play(AudioManager.instance.sounds[Random.Range(2, 14)].name);
    }
}
