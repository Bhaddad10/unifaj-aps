using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameObject follows player, destroying unused obstacles
public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // If its a PowerUp, only destroy it if not consumed by player
        //    (or else deactivate would not happen)
        if (collision.CompareTag("PowerUp")) {
            if (!collision.gameObject.GetComponent<SneakersPowerUp>().isActive)
            {
                Destroy(collision.gameObject);
            }
        }

        // If its an obstacle or coin, destroy normally
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Coin"))
        {
            Destroy(collision.transform.root.gameObject);
        }
    }
}
