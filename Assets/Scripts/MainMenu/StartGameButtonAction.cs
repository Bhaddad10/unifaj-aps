using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonAction : MonoBehaviour
{
    public string gameScene;

    // Stops theme
    private void Awake()
    {
        if (AudioManager.Instance)
            AudioManager.Instance.Stop("GameTheme");
    }

    // Start playing HomeTheme
    private void Start()
    {
        AudioManager.Instance.Play("HomeTheme");
    }

    // OnClick StartGame
    public void StartGame()
    {
        // Play UI sound, stop MainMenu theme and unpause
        AudioManager.Instance.Play("uiClick");
        AudioManager.Instance.Stop("HomeTheme");
        Time.timeScale = 1f;

        // Load game
        SceneManager.LoadScene(gameScene);
    }
}
