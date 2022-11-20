using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonAction : MonoBehaviour
{
    public string gameScene;

    private void Awake()
    {
        if (AudioManager.instance)
            AudioManager.instance.Stop("GameTheme");
    }

    private void Start()
    {
        AudioManager.instance.Play("HomeTheme");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
        AudioManager.instance.Play("uiClick");
        AudioManager.instance.Stop("HomeTheme");
        Time.timeScale = 1f;
    }
}
