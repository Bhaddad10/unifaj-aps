using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;

    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask ground;

    public float speed = 10f;
    public float jumpForce = 5f;
    Vector3 posicaoInicial; // posição inicial do player
    
    // variável pública para poder inspecionar no unity
    public int coins = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        posicaoInicial = transform.position;
    }
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rigidbody.velocity = new Vector3(horizontal * speed, rigidbody.velocity.y, vertical * speed);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            jump();
        }
    }

    void jump()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
    }

    bool isGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, 0.1f, ground);
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
;
        if (other.CompareTag("Spike"))
        {
            ResetLevel();
        }

        if (other.CompareTag("Coin"))
        {
            GetCoin(other);
        }

        if (other.CompareTag("Finish"))
        {
            
        }
    }

    private void ResetLevel()
    {
        transform.position = posicaoInicial;
    }

    private void GetCoin(GameObject other)
    {
        Destroy(other);
        coins++;
    }
}
