using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    //Variavel para auxiliar a mudança de posição da câmera para melhorar o angulo
    public Vector3 offset;

    void Update()
    { 
        //Define a posição da câmera como o objeto que for referenciado
        transform.position = target.position + offset;
    }  
}
