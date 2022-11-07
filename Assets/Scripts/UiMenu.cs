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
        GameManager.Instance.Init(EndingDialog, InGameInfoDialog);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseDialog.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseDialog.SetActive(false);
    }
}

