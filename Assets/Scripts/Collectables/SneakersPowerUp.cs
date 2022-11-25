using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles powerup logic
public class SneakersPowerUp : MonoBehaviour
{
    // default jump force
    private static float normalJump;
    // the deactivateCoroutine running
    private static IEnumerator deactivateCoroutine;
    // powerUp gameObject, if the user gets it
    private static GameObject activePowerUpGameObject;
    // The time the last powerUp was picked up
    public static float lastPowerUp;
    // Stores the duration of the powerUp in a static variable (InGameUI consumes this)
    public static float sDuration;
    
    // handles 'duringCollision'
    public bool hasCollidedAlready = false;
    // if user has activated this powerUp
    public bool isActive = false;

    // PowerUp duration
    [Range(1f, 8f)]
    public float duration = 2f;
    // PowerUp value
    public float jumpBoost;

    // Stores PowerUp's visible element
    public GameObject Visual;

    private void Start()
    {
        // set helper values
        sDuration = duration;
        if (normalJump == 0f)
            normalJump = GameManager.Instance.player.jumpForce;
    }

    public void ActivatePowerUp()
    {
        // If user collided twice with same power up, return
        if (hasCollidedAlready) return;
        hasCollidedAlready = true;

        lastPowerUp = Time.time;
        
        // if there's already a powerUp on, destroy it (restarting timer)
        if (GameManager.Instance.player.isSneakersPowerUpOn)
        {
            StopCoroutine(deactivateCoroutine);
            Destroy(activePowerUpGameObject);
        }

        //Debug.Log("ActivatePowerUp");
        // Activate it
        GameManager.Instance.player.isSneakersPowerUpOn = true;
        GameManager.Instance.player.jumpForce = normalJump * jumpBoost;

        // Schedule deactivation
        deactivateCoroutine = DeactivatePowerUp(duration);
        activePowerUpGameObject = gameObject;
        StartCoroutine(deactivateCoroutine);

        isActive = true;

        // Hide visual elements
        Visual.SetActive(false);
    }

    IEnumerator DeactivatePowerUp(float durationInSeconds)
    {
        //Debug.Log("DeactivatePowerUp - " + durationInSeconds);
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(durationInSeconds);

        //Debug.Log("After " + durationInSeconds);
        // Restore jump force and destroy GameObject
        GameManager.Instance.player.jumpForce = normalJump;
        GameManager.Instance.player.isSneakersPowerUpOn = false;
        Destroy(gameObject);
    }

}
