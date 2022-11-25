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
    public GameObject PowerUp;
    public GameObject PowerUpBg;
    private Image PowerUpImage;

    private void Start()
    {
        PowerUpImage = PowerUp.GetComponent<Image>();
    }

    internal void SetInfo(int score, int coins, int multiplier)
    {
        ScoreValueTxt.text = score.ToString();
        CoinsValueTxt.text = coins.ToString();
        MultiplierValueTxt.text = multiplier.ToString() + "x";

    }

    internal void SetPowerUpInfo(bool powerUp = false, float percentage = 0)
    {
        PowerUp.SetActive(powerUp);
        PowerUpBg.SetActive(powerUp);
        if (powerUp)
        {
            PowerUpImage.fillAmount = 1 - percentage;
        }
    }
}
