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

    internal void SetInfo(int score, int coins, int multiplier)
    {
        ScoreValueTxt.text = score.ToString();
        CoinsValueTxt.text = coins.ToString();
        MultiplierValueTxt.text = multiplier.ToString() + "x";
    }
}
