using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoPowerUp : MonoBehaviour
{
    public Vector3 speed;

    void FixedUpdate()
    {
        // Rotaciona a moeda
        transform.Rotate(speed);
    }
}
