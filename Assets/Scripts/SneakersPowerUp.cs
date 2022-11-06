using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakersPowerUp : MonoBehaviour
{
    [Range(1f, 8f)]
    public float duration = 2f;
    public float jumpBoost;

    private float normalJump;

    public GameObject Visual;

    private void Start()
    {
        normalJump = GameManager.Instance.player.jumpForce;
    }

    public void ActivatePowerUp()
    {
        if (GameManager.Instance.player.isSneakersPowerUpOn)
            return;
        GameManager.Instance.player.isSneakersPowerUpOn = true;
        Debug.Log("ActivatePowerUp");
        GameManager.Instance.player.jumpForce = normalJump * jumpBoost;

        StartCoroutine(DeactivatePowerUp(duration));

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
