using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonAction : MonoBehaviour
{
    public string gameScene;

    private void Awake()
    {
        if (AudioManager.Instance)
            AudioManager.Instance.Stop("GameTheme");
    }

    private void Start()
    {
        AudioManager.Instance.Play("HomeTheme");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
        AudioManager.Instance.Play("uiClick");
        AudioManager.Instance.Stop("HomeTheme");
        Time.timeScale = 1f;
    }
}
