using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UiMenu : MonoBehaviour
{
    public GameObject EndingDialog;

    private void Start()
    {
        GameManager.Instance.SetAndHideEndingDialog(EndingDialog);
    }
}

