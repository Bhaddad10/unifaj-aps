using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamera : MonoBehaviour
{
    // Objeto a ser seguido
    public GameObject targetObject;
    // Variavel para auxiliar a mudança de posição da câmera para melhorar o ângulo
    private Vector3 offset;

    void Start()
    {
        // Salva o offset entre o objeto e a câmera
        offset = transform.position - targetObject.transform.position;
    }

    // Atualiza a posição da câmera com base no objeto referenciado
    void Update()
    {
        transform.position = targetObject.transform.position + offset;
    }
}
