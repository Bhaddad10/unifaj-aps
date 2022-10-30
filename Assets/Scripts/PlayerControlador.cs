using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlador: MonoBehaviour
{
    Rigidbody _rigidbody;

    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask ground;

    public float speed = 10f; // velocidade do player
    public float jumpForce = 5f; // força do pulo
    Vector3 posicaoInicial; // posição inicial do player
    
    public int coins = 0; // variável pública para poder inspecionar no unity
    public Text finishText; // elemento de texto de vitória
    public Text coinsText; // elemento de texto de moedas coletadas


    private int currentLane = 1;
    float[] lanes = { -6.5f, 0f, 6.5f };

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // armazena posição inicial para possível reset
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Movimentação Automática para Frente
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, speed);


        // Controles
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movimentação entre as faixas
        // Se setas esquerda/direita, aplique a alteração de lane
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow))  ChangeLane(+1);
        transform.position = Vector3.Lerp(transform.position, new Vector3(lanes[currentLane], transform.position.y, transform.position.z), .5f);

        //transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);


        // Pulo do player
        // Se está pressionando botão de pulo (ou setas para cima), e não está no chão
        if ((Input.GetButtonDown("Jump") || vertical == 1) && isGrounded())
        {
            jump();
        }
    }

    // Método para evitar que a currentLane ultrapasse os limites (0 e 2)
    private void ChangeLane(int diff)
    {
        // Indo para lane da esquerda
        if (diff < 0)
            currentLane = Math.Max(currentLane + diff, 0);

        // Indo para lane da direita
        if (diff > 0)
            currentLane = Math.Min(currentLane + diff, 2);
    }

    // Método que realiza o pulo do player
    void jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
    }

    // Método retorna se player está no chão
    bool isGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, 0.1f, ground);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Ao colidir com um objeto
        GameObject other = collision.gameObject;
;

        // Se for um espinho, reseta o jogador para posição inicial
        if (other.CompareTag("Spike"))
        {
            ResetLevel();
        }

        // Se for uma moeda, coleta-a
        if (other.CompareTag("Coin"))
        {
            GetCoin(other);
        }

        // Se for a linha de chegada, exibe texto de finalização da fase
        if (other.CompareTag("Finish"))
        {
            coinsText.text = coinsText.text.Replace("{coins}", coins.ToString());
            coinsText.gameObject.SetActive(true);
            finishText.gameObject.SetActive(true);
        }
    }

    private void ResetLevel()
    {
        transform.position = posicaoInicial;
    }

    // destrói a moeda e adiciona ao contador de moedas do player
    private void GetCoin(GameObject other)
    {
        Destroy(other);
        coins++;
    }
}
