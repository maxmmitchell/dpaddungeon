using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI score;

    void Start()
    {
        score.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }

    public void LoadMenu()
    {
        FindObjectOfType<SceneLoader>().LoadNextScene("MainMenu");
    }
}
