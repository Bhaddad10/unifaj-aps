using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class GameManager : IPersistentSingleton<GameManager>
{
    public GameObject EndingDialog;
    GameObject InGameInfoDialog;

    public void Init(GameObject newEndingDialog, GameObject newInGameInfoDialog)
    {
        SetAndHideEndingDialog(newEndingDialog);

        InGameInfoDialog = newInGameInfoDialog;
        UpdateInGameInfoDialog(0, 0, 1);
    }

    public void SetAndHideEndingDialog(GameObject newEndingDialog)
    {
        EndingDialog = newEndingDialog;

        Debug.Log("EndingDialog=" + EndingDialog);
        Instance.HideEndingDialog();
    }

    public void ShowEndingDialog(int score, int coins)
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(true);
        EndingDialog.GetComponent<FinishedLevelDialogConfig>().SetInfo(score, coins);
    }

    public void HideEndingDialog()
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(false);
    }

    internal void UpdateInGameInfoDialog(int score, int coins, int multiplier)
    {
        InGameInfoDialog.GetComponent<InGameInfoDialogConfig>().SetInfo(score, coins, multiplier);
    }
}
