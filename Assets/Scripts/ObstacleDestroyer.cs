using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PowerUp")) {
            if (!collision.gameObject.GetComponent<SneakersPowerUp>().isActive)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("Obstacle") || collision.CompareTag("Coin"))
        {
            Destroy(collision.transform.root.gameObject);
        }
    }
}
