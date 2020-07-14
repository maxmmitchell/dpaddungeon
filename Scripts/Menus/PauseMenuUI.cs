using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public bool GameIsPaused = false;

    public static PauseMenuUI instance;

    public GameObject pauseMenuUI;

    public bool pauseable = true;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        // pausing is not allowed on the main menu or level select scenes
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "ScoreScene" && pauseable)
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        FindObjectOfType<AudioManager>().PlayButtonSFX();

        FindObjectOfType<AudioManager>().UnPauseAll();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("SFXUnpause", 1f);
        GameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        FindObjectOfType<AudioManager>().PauseAll();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("SFXPause", 1f);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;  
    }

    public void LoadMenu()
    {
        Resume();
        FindObjectOfType<AudioManager>().PauseAll();
        FindObjectOfType<AudioManager>().PlayButtonSFX();
        if (FindObjectOfType<AudioManager>().musicPlaying.Substring(0, 5) == "Music")
        {
            FindObjectOfType<AudioManager>().Stop(FindObjectOfType<AudioManager>().musicPlaying);
            //FindObjectOfType<AudioManager>().UnPause(FindObjectOfType<AudioManager>().musicPlaying);
        }

        FindObjectOfType<SceneLoader>().LoadNextScene("MainMenu");
    }

    public void LoadControls()
    {
        pauseable = false;
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayButtonSFX()
    {
        AudioManager.instance.PlayButtonSFX();
    }
}
