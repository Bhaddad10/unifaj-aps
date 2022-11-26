using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameInfoDialogConfig : MonoBehaviour
{
    public Text ScoreValueTxt;
    public Text CoinsValueTxt;
    public Text MultiplierValueTxt;
    
    // Parent element Power Up
    public GameObject PowerUp;
    // image
    private Image PowerUpImage;
    // timer background
    public GameObject PowerUpBg;

    private void Start()
    {
        // set image reference
        PowerUpImage = PowerUp.GetComponent<Image>();
    }

    // Update score, coins and multiplier info
    internal void SetInfo(int score, int coins, int multiplier)
    {
        ScoreValueTxt.text = score.ToString();
        CoinsValueTxt.text = coins.ToString();
        MultiplierValueTxt.text = multiplier.ToString() + "x";

    }

    // Updates PowerUp timer: percentage and show/hide
    internal void SetPowerUpInfo(bool powerUp = false, float percentage = 0)
    {
        // Show/hide PowerUp
        PowerUp.SetActive(powerUp);
        PowerUpBg.SetActive(powerUp);

        // Updates timer
        if (powerUp)
        {
            PowerUpImage.fillAmount = 1 - percentage;
        }
    }
}
