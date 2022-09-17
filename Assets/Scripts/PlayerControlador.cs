using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlador: MonoBehaviour
{
    Rigidbody rigidbody;

    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask ground;

    public float speed = 10f; // velocidade do player
    public float jumpForce = 5f; // força do pulo
    Vector3 posicaoInicial; // posição inicial do player
    
    public int coins = 0; // variável pública para poder inspecionar no unity
    public Text finishText; // elemento de texto de vitória

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // armazena posição inicial para possível reset
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Movimentação do Player
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rigidbody.velocity = new Vector3(horizontal * speed, rigidbody.velocity.y, vertical * speed);

        // Pulo do player
        // Se está pressionando botão de pulo, e não está no chão
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            jump();
        }
    }

    // Método que realiza o pulo do player
    void jump()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
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
