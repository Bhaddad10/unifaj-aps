using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Coin") || collision.CompareTag("PowerUp"))
        {
            Debug.Log("Collided with obs or coin");
            Destroy(collision.transform.root.gameObject);
        }
    }
}
