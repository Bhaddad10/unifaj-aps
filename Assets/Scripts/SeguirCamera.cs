using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamera : MonoBehaviour
{
    // Objeto a ser seguido
    public Transform target;
    // Variavel para auxiliar a mudança de posição da câmera para melhorar o ângulo
    public Vector3 offset;

    void Update()
    { 
        // Atualiza a posição da câmera com base no objeto referenciado
        transform.position = target.position + offset;
    }  
}
