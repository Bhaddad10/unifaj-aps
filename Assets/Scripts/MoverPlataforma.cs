using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverPlataforma : MonoBehaviour
{
    public float velocidade = 2.0f; // velocidade da plataforma
    public float direcao = 1.0f; // começa positiva
    float mov;
    Vector3 posicaoInicial; // posição inicial da plataforma
    public float positiveRange = 1f, negativeRange = -1f;

    // Start is called before the first frame update
    void Start()
    {
        posicaoInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mov = velocidade * Time.deltaTime * direcao;

        // nova posicao de plataforma no eixo especifico
        float nova_pos = transform.position.x + mov;

        if (nova_pos >= positiveRange ||
            nova_pos <= negativeRange)
        {
            direcao *= -1; // invertemos a direção do movimento
        }
        else
        {
            transform.position += new Vector3(mov, 0, 0);
        }
    }
}
