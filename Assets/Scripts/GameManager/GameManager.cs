using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class GameManager : IPersistentSingleton<GameManager>
{
    public GameObject EndingDialog;

    public void SetAndHideEndingDialog(GameObject newEndingDialog)
    {
        EndingDialog = newEndingDialog;

        Debug.Log("EndingDialog=" + EndingDialog);
        Instance.HideEndingDialog();
    }

    public void ShowEndingDialog()
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(true);
    }

    public void HideEndingDialog()
    {
        if (EndingDialog == null)
            return;
        EndingDialog.SetActive(false);
    }

}
