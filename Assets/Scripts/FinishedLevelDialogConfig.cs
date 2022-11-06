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

    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene(NextLevelScene);
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene(HomeScene);
    }

    internal void SetInfo(int score, int coins)
    {
        ScoreValueTxt.text = score.ToString();
        CoinsValueTxt.text = coins.ToString();
    }
}
