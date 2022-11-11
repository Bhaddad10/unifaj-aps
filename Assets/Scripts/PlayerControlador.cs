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
    private Vector3 targetScale;

    private IEnumerator getBackUpCoroutine;
    private bool isJumping;
    private bool isDashingDown;
    private int score = 0;
    private int multiplier = 1;
    
    public bool isSneakersPowerUpOn;

    [Range(1f, 100f)]
    public float increaseSpeed;

    private void Start()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;

        _rigidbody = GetComponent<Rigidbody>();
        // armazena posi��o inicial para poss�vel reset
        posicaoInicial = transform.position;

        InvokeRepeating("Walk", .25f, .25f);
        InvokeRepeating("ScoreCount", .05f, .05f);
    }
    private void Update()
    {
        // Controles
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movimenta��o entre as faixas
        // Se setas esquerda/direita, aplique a altera��o de lane
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow))  ChangeLane(+1);

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
            isJumping = true;
        }

        // Se est� pressionando seta baixo, e n�o est� no ch�o
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            isDashingDown = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded() && !isDucking)
        {
            isDucking = true;
            targetScale = normalScale * .5f;
            DoGetBackUp(duckingTime);
        }

        if (transform.localScale.y != targetScale.y)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, targetScale.y, 10f * Time.deltaTime), transform.localScale.z);
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


    // M�todo retorna se player est� no ch�o
    bool isGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, 0.1f, ground);
    }

    private void FixedUpdate()
    {
        // Movimenta��o Autom�tica para Frente
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, speed);

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, lanes[currentLane], .2f), transform.position.y, transform.position.z);



        if (isJumping)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
            isJumping = false;
        }

        if (isDashingDown)
        {
            _rigidbody.AddForce(new Vector3(0, -dashDownAcceleration, 0), ForceMode.VelocityChange);
            isDashingDown = false;
        }

        // Cair mais r�pido
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }


    private void OnTriggerEnter(Collider collision)
    {
        // Ao collidir trigger com um objeto
        GameObject other = collision.gameObject;

        // Se for uma moeda, coleta-a
        if (other.CompareTag("Coin"))
        {
            GetCoin(other);
        }

        if (other.CompareTag("PowerUp"))
        {
            GetPowerUp(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ao colidir com um objeto
        GameObject other = collision.gameObject;

        // Se for um espinho, reseta o jogador para posi��o inicial
        if (other.CompareTag("Spike") || other.CompareTag("Obstacle"))
        {
            ResetLevel();
        }

        // Se for a linha de chegada, exibe texto de finaliza��o da fase
        if (other.CompareTag("Finish"))
        {
            //coinsText.text = coinsText.text.Replace("{coins}", coins.ToString());
            //coinsText.gameObject.SetActive(true);
            //finishText.gameObject.SetActive(true);
            GameManager.Instance.ShowEndingDialog(score, coins);
            //AudioManager.instance.Play("dieSound");
            //StartCoroutine(DoPlayDie());
            //speed = 0;
        }
    }

    private void ResetLevel()
    {
        //transform.position = posicaoInicial;
        AudioManager.instance.StopAll();
        StartCoroutine(DoPlayDie());
        speed = 0;
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<BoxCollider>());
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        // stop walk sound
        CancelInvoke("Walk");
        CancelInvoke("ScoreCount");
    }

    // destr�i a moeda e adiciona ao contador de moedas do player
    private void GetCoin(GameObject other)
    {
        Destroy(other);
        coins++;
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        AudioManager.instance.Play("getCoin");
    }

    private void GetPowerUp(GameObject other)
    {
        other.GetComponent<SneakersPowerUp>().ActivatePowerUp();
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        AudioManager.instance.Play("getPowerUp");
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
        isDucking = false;
    }

    void Walk()
    {
        if (isGrounded())
            AudioManager.instance.PlayWalk();
    }

    void ScoreCount()
    {
        score += 1;
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        speed += increaseSpeed/100;
    }

    IEnumerator DoPlayDie()
    {
        AudioManager.instance.Play("dieSound");
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(2f);
        AudioManager.instance.Play("die");

        yield return new WaitForSeconds(2f);
        AudioManager.instance.Play("loose");
        GameManager.Instance.ShowEndingDialog(score, coins);
    }
}
