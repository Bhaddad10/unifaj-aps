using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotates the gameObject containing this component by a value defined on Editor
public class RotacaoPowerUp : MonoBehaviour
{
    public Vector3 speed;

    void FixedUpdate()
    {
        // Rotates the gameObject
        transform.Rotate(speed);
    }
}
