using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject targetObject;
    // offset
    private Vector3 offset;

    void Start()
    {
        // saves current offset
        offset = transform.position - targetObject.transform.position;
    }

    // follows target
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, targetObject.transform.position.z + offset.z);
    }
}
