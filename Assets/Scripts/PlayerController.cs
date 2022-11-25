using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController: MonoBehaviour
{
    // Player parameters
    [Range(1f, 25f)]
    public float speed = 10f; // player's speed
    [Range(1f, 25f)]
    public float jumpForce = 5f;
    [Range(1f, 25f)]
    public float dashDownAcceleration; // dashDown force
    [Range(1f, 3f)]
    public float fallMultiplier = 2.5f; // fall faster
    [Range(0.1f, 5f)]
    public float duckingTime = 1.5f; // ducking duration
    // Speed increases over time
    [Range(1f, 100f)]
    public float increaseSpeed;

    // logic
    Rigidbody _rigidbody;
    // attributes for checking if player is onGround
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask ground;

    // player stats
    private int coins = 0;
    private int score = 0;
    private int multiplier = 1;
    [HideInInspector]
    public bool isSneakersPowerUpOn;

    // Handles player death 'animation'
    public GameObject playerBody;
    public GameObject playerDeadBody;

    // Duck compress
    private Vector3 normalScale;
    private Vector3 targetScale;
    // Scheduled de-compress
    private IEnumerator getBackUpCoroutine;

    // Lane-switching
    float[] lanes = { -6.5f, 0f, 6.5f };
    private int currentLane = 1;

    // Control State
    private bool isDucking = false;
    private bool isJumping;
    private bool isDashingDown;
    private bool isDead;

    private void Start()
    {
        // set default values
        normalScale = transform.localScale;
        targetScale = normalScale;

        _rigidbody = GetComponent<Rigidbody>();

        // invoke repeating walk-sounds
        // and score count
        InvokeRepeating("Walk", .25f, .25f);
        InvokeRepeating("ScoreCount", .05f, .05f);
    }
    private void Update()
    {
        // if is dead, ignore input
        if (isDead)
            return;

        // Controls
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Lane-switching
        // If left/right arrows, change lane
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow))  ChangeLane(+1);

        //transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        // Jump
        // If Jump button, and is grounded, jump
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded())
        {
            // if ducking, cancel ducking before jumping
            if (isDucking)
            {
                // override get-backup schedule to now
                StopCoroutine(getBackUpCoroutine);
                GetBackUp();
            }
            isJumping = true;
        }

        // If down arrow and jumping, dash down
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded())
        {
            isDashingDown = true;
        }

        // if down arrow, but grounded, duck
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded() && !isDucking)
        {
            isDucking = true;
            targetScale = normalScale * .5f;
            DoGetBackUp(duckingTime);
        }

        // resize player
        if (transform.localScale.y != targetScale.y)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, targetScale.y, 10f * Time.deltaTime), transform.localScale.z);
        }
    }

    // Change lanes (limit to 0 to 2)
    private void ChangeLane(int diff)
    {
        // Going left
        if (diff < 0)
            currentLane = Math.Max(currentLane + diff, 0);

        // Going right
        if (diff > 0)
            currentLane = Math.Min(currentLane + diff, 2);
    }


    // is player on ground
    bool isGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, 0.1f, ground);
    }

    private void FixedUpdate()
    {
        // Move forward
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, speed);

        // changing lanes 'animation'
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, lanes[currentLane], .2f), transform.position.y, transform.position.z);

        // process jumping/dashing down requests:
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

        // Fall faster
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }


    private void OnTriggerEnter(Collider collision)
    {
        // On Trigger
        GameObject other = collision.gameObject;

        // collect coin/powerup
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
        // On Collide
        GameObject other = collision.gameObject;

        // if obstacle, defeat
        if (other.CompareTag("Obstacle"))
        {
            Defeat();
        }
    }

    private void Defeat()
    {
        // stop sounds
        AudioManager.Instance.StopAll();

        // play die sounds
        StartCoroutine(DoPlayDie());

        // stop coroutines
        CancelInvoke("Walk");
        CancelInvoke("ScoreCount");

        // stop player altogether
        speed = 0;
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<BoxCollider>());
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        isDead = true;
        GetComponentInChildren<Animator>().enabled = false;
    }

    private void GetCoin(GameObject other)
    {
        Destroy(other);
        coins++;
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        AudioManager.Instance.Play("getCoin");
    }

    private void GetPowerUp(GameObject other)
    {
        other.GetComponent<SneakersPowerUp>().ActivatePowerUp();
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        AudioManager.Instance.Play("getPowerUp");
    }


    void DoGetBackUp(float delayTime)
    {
        getBackUpCoroutine = GetBackUpEnumerator(delayTime);
        StartCoroutine(getBackUpCoroutine);
    }

    IEnumerator GetBackUpEnumerator(float delayTime)
    {
        // Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        GetBackUp();
    }

    void GetBackUp()
    {
        // Get back up after the delay time has finished
        targetScale = normalScale;
        isDucking = false;
    }

    // InvokedRepeating
    void Walk()
    {
        // play a footstep if on ground
        if (isGrounded())
            AudioManager.Instance.PlayWalk();
    }

    // InvokedRepeating
    void ScoreCount()
    {
        // count and update score
        score += 1;
        GameManager.Instance.UpdateInGameInfoDialog(score, coins, multiplier);
        GameManager.Instance.UpdateInGamePowerUpInfoDialog();
        speed += increaseSpeed/100;
    }

    IEnumerator DoPlayDie()
    {
        // play die theme
        AudioManager.Instance.Play("dieSound");
        
        // Wait 2 seconds
        yield return new WaitForSeconds(2f);
        // play kill & show body
        AudioManager.Instance.Play("die");
        playerBody.SetActive(false);
        playerDeadBody.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);
        // Play loose theme & show dialog
        AudioManager.Instance.Play("loose");
        GameManager.Instance.ShowEndingDialog(score, coins);

    }
}
