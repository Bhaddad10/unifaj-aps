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
    
    public float jumpForce = 5f; // for�a do pulo
    public float fallMultiplier = 2.5f;

    Vector3 posicaoInicial; // posi��o inicial do player
    
    public int coins = 0; // vari�vel p�blica para poder inspecionar no unity
    public Text finishText; // elemento de texto de vit�ria
    public Text coinsText; // elemento de texto de moedas coletadas


    private int currentLane = 1;
    float[] lanes = { -6.5f, 0f, 6.5f };
    
    private bool isDucking = false;
    [Range(0.1f, 5f)]
    public float duckingTime = 1.5f;
    public float dashDownAcceleration;

    private Vector3 normalScale;
    private Vector3 normalPosition;
    private Vector3 targetScale;
    private Vector3 targetPosition;
    private IEnumerator getBackUpCoroutine;

    private void Start()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;

        normalPosition = transform.localPosition;
        targetPosition = normalPosition;

        _rigidbody = GetComponent<Rigidbody>();
        // armazena posi��o inicial para poss�vel reset
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Movimenta��o Autom�tica para Frente
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, speed);


        // Controles
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movimenta��o entre as faixas
        // Se setas esquerda/direita, aplique a altera��o de lane
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow))  ChangeLane(+1);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, lanes[currentLane], 10f * Time.deltaTime), transform.position.y, transform.position.z);

        //transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);


        // Pulo do player
        // Se est� pressionando bot�o de pulo (ou setas para cima), e est� no ch�o
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded())
        {
            if (isDucking)
            {
                Debug.Log("Jumping when ducking");
                StopCoroutine(getBackUpCoroutine);
                GetBackUp();
            }
            jump();
        }

        // Cair mais r�pido
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Se est� pressionando seta baixo, e n�o est� no ch�o
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            _rigidbody.AddForce(new Vector3(0, -dashDownAcceleration, 0), ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded() && !isDucking)
        {
            isDucking = true;
            targetScale = normalScale * .5f;
            targetPosition = normalPosition * .5f;
            DoGetBackUp(duckingTime);
        }

        if (transform.localScale.y != targetScale.y)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, targetScale.y, 10f * Time.deltaTime), transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, targetPosition.y, 10f * Time.deltaTime), transform.localPosition.z);
        }
    }

    // M�todo para evitar que a currentLane ultrapasse os limites (0 e 2)
    private void ChangeLane(int diff)
    {
        // Indo para lane da esquerda
        if (diff < 0)
            currentLane = Math.Max(currentLane + diff, 0);

        // Indo para lane da direita
        if (diff > 0)
            currentLane = Math.Min(currentLane + diff, 2);
    }

    // M�todo que realiza o pulo do player
    void jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
    }

    // M�todo retorna se player est� no ch�o
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

        // Se for um espinho, reseta o jogador para posi��o inicial
        if (other.CompareTag("Spike") || other.CompareTag("Obstacle"))
        {
            ResetLevel();
        }

        // Se for uma moeda, coleta-a
        if (other.CompareTag("Coin"))
        {
            GetCoin(other);
        }

        // Se for a linha de chegada, exibe texto de finaliza��o da fase
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

    // destr�i a moeda e adiciona ao contador de moedas do player
    private void GetCoin(GameObject other)
    {
        Destroy(other);
        coins++;
    }


    void DoGetBackUp(float delayTime)
    {
        getBackUpCoroutine = GetBackUpEnumerator(delayTime);
        StartCoroutine(getBackUpCoroutine);
    }

    IEnumerator GetBackUpEnumerator(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        GetBackUp();
    }

    void GetBackUp()
    {
        //Do the action after the delay time has finished.
        targetScale = normalScale;
        targetPosition = normalPosition;
        isDucking = false;
    }
}
