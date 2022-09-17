using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoMoeda : MonoBehaviour
{
    void Update()
    {
        // Rotaciona a moeda
        transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
    }
}
