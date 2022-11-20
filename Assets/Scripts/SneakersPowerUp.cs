using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakersPowerUp : MonoBehaviour
{
    private static float normalJump;
    private static IEnumerator deactivateCoroutine;
    private static GameObject activePowerUpGameObject;
    public static float lastPowerUp;
    public static float sDuration;
    public bool hasCollidedAlready = false;
    public bool isActive = false;

    [Range(1f, 8f)]
    public float duration = 2f;
    public float jumpBoost;

    public GameObject Visual;

    private void Start()
    {
        sDuration = duration;
        if (normalJump == 0f)
            normalJump = GameManager.Instance.player.jumpForce;
    }

    public void ActivatePowerUp()
    {
        if (hasCollidedAlready) return;
        hasCollidedAlready = true;

        lastPowerUp = Time.time;
        if (GameManager.Instance.player.isSneakersPowerUpOn)
        {
            StopCoroutine(deactivateCoroutine);
            Destroy(activePowerUpGameObject);
        }

        GameManager.Instance.player.isSneakersPowerUpOn = true;
        Debug.Log("ActivatePowerUp");
        GameManager.Instance.player.jumpForce = normalJump * jumpBoost;

        deactivateCoroutine = DeactivatePowerUp(duration);
        activePowerUpGameObject = gameObject;
        StartCoroutine(deactivateCoroutine);

        isActive = true;
        Visual.SetActive(false);
    }

    IEnumerator DeactivatePowerUp(float durationInSeconds)
    {
        Debug.Log("DeactivatePowerUp - " + durationInSeconds);
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(durationInSeconds);

        Debug.Log("After " + durationInSeconds);
        GameManager.Instance.player.jumpForce = normalJump;
        GameManager.Instance.player.isSneakersPowerUpOn = false;
        Destroy(gameObject);
    }

}
