using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishedLevelDialogConfig : MonoBehaviour
{
    public Text ScoreValueTxt;
    public Text CoinsValueTxt;
    public string NextLevelScene;
    public string HomeScene;

    // When clicking RestartButton, play UI sound and restart level
    public void OnRestartButtonClicked()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("uiClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // When clicking HomeButton, play UI sound and load home
    public void OnHomeButtonClicked()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("uiClick");
        SceneManager.LoadScene(HomeScene);
    }

    // Set score and coins value
    internal void SetInfo(int score, int coins)
    {
        ScoreValueTxt.text = score.ToString();
        CoinsValueTxt.text = coins.ToString();
    }
}
