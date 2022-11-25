using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotates the gameObject containing this component by a value defined on Editor
public class RotacaoMoeda : MonoBehaviour
{
    void Update()
    {
        // Rotates coin
        transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
    }
}
