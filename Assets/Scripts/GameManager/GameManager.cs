using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class GameManager : IPersistentSingleton<GameManager>
{
    // GUI Dialogs (GameManager is responsible for showing/hiding these dialogs during game)
    GameObject EndingDialog;
    GameObject InGameInfoDialog;
    
    // PlayerController reference, so scripts can get its stats
    public PlayerControlador player;

    private void Start()
    {
        // When game is started, ambient and/or theme starts
        AudioManager.Instance.Play("AmbientHallway");
        AudioManager.Instance.Play("GameTheme");
    }

    // UiMenu call this method on Start of scene, populating the dialogs to the right value
    public void Init(GameObject newEndingDialog, GameObject newInGameInfoDialog)
    {
        SetAndHideEndingDialog(newEndingDialog);
        InGameInfoDialog = newInGameInfoDialog;

        // Updates Game Stats Dialog values (score, coins and multiplier)
        UpdateInGameInfoDialog(0, 0, 1);
    }

    // Sets EndingDialog, and hides it
    public void SetAndHideEndingDialog(GameObject newEndingDialog)
    {
        EndingDialog = newEndingDialog;
        //Debug.Log("EndingDialog=" + EndingDialog);
        Instance.HideEndingDialog();
    }

    // Hides EndingDialog
    public void HideEndingDialog()
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(false);
    }

    // Shows EndingDialog
    public void ShowEndingDialog(int score, int coins)
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(true);
        // Ask for stat update on Finished Dialog
        EndingDialog.GetComponent<FinishedLevelDialogConfig>().SetInfo(score, coins);
    }

    // Shows EndingDialog
    internal void UpdateInGameInfoDialog(int score, int coins, int multiplier)
    {
        // Updates InGameDialog stats
        InGameInfoDialog.GetComponent<InGameInfoDialogConfig>().SetInfo(score, coins, multiplier);
    }

    // Updates PowerUp timer
    internal void UpdateInGamePowerUpInfoDialog()
    {
        // Calculates percentage to fill PowerUp circle
        float percentage = (Time.time - SneakersPowerUp.lastPowerUp) / SneakersPowerUp.sDuration;
        // Updates PowerUp Info
        InGameInfoDialog.GetComponent<InGameInfoDialogConfig>().SetPowerUpInfo(player.isSneakersPowerUpOn, percentage);
    }
}
