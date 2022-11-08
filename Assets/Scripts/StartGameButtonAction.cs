using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonAction : MonoBehaviour
{
    public string gameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
        AudioManager.instance.Play("uiClick");
        Time.timeScale = 1f;
    }
}
