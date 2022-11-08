using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonAction : MonoBehaviour
{
    public string gameScene;

    public void StartGame()
    {
        Debug.Log("ué");
        SceneManager.LoadScene(gameScene);
    }
}
