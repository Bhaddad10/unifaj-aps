using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UiMenu : MonoBehaviour
{
    public GameObject EndingDialog;
    public GameObject InGameInfoDialog;
    public GameObject PauseDialog;

    private void Start()
    {
        // Pass dialogs to GameManager
        GameManager.Instance.Init(EndingDialog, InGameInfoDialog);
    }

    public void Pause()
    {
        // Pause game and show PauseDialog
        Time.timeScale = 0f;
        PauseDialog.SetActive(true);
        AudioManager.Instance.Play("uiClick");
    }

    public void Resume()
    {
        // Resume game and hide PauseDialog
        Time.timeScale = 1f;
        PauseDialog.SetActive(false);
        AudioManager.Instance.Play("uiClick");
    }
}

