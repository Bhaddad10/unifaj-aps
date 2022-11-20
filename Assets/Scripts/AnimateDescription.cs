using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDescription : MonoBehaviour
{
    [Range(1f, 10f)]
    public float range = 5f;

    [Range(1f, 5f)]
    public float speed = 1f;

    void FixedUpdate()
    {        
        transform.Rotate(Vector3.forward * Mathf.Sin(Time.time * (speed/2)) * (range / 10));
        transform.localScale += (Vector3.up + Vector3.right) * (Mathf.Sin(Time.time * (speed / 2)) * (range / 10))/80;
    }
}
