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
    public float fallMultiplier = 2.5f;

    Vector3 posicaoInicial; // posição inicial do player
    
    public int coins = 0; // variável pública para poder inspecionar no unity
    public Text finishText; // elemento de texto de vitória
    public Text coinsText; // elemento de texto de moedas coletadas


    private int currentLane = 1;
    float[] lanes = { -6.5f, 0f, 6.5f };
    
    private bool isDucking = false;

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
        transform.position = Vector3.Lerp(transform.position, new Vector3(lanes[currentLane], transform.position.y, transform.position.z), 10f * Time.deltaTime);

        //transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);


        // Pulo do player
        // Se está pressionando botão de pulo (ou setas para cima), e está no chão
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded())
        {
            jump();
        }

        // Cair mais rápido
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Se está pressionando seta baixo, e não está no chão
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            _rigidbody.AddForce(new Vector3(0, -15, 0), ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded() && !isDucking)
        {
            isDucking = true;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * .5f, transform.localScale.z);
            DoGetBackUp(2f);
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

    /*private void FixedUpdate()
    {
        if (!isGrounded() && _rigidbody.velocity.y <= 0)
        {
            _rigidbody.AddForce(new Vector3(0, -.3f, 0), ForceMode.VelocityChange);
        }
    }*/



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


    void DoGetBackUp(float delayTime)
    {
        StartCoroutine(GetBackUp(delayTime));
    }

    IEnumerator GetBackUp(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2f, transform.localScale.z);
        isDucking = false;
    }
}
