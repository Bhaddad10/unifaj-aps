using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UiMenu : MonoBehaviour
{
    public GameObject EndingDialog;
    public GameObject InGameInfoDialog;

    private void Start()
    {
        GameManager.Instance.Init(EndingDialog, InGameInfoDialog);
    }
}

