using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component useful for showing gameObject's position using gizmo
public class Gizmo : MonoBehaviour
{
    public float gizmoSize = .75f;
    public Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoSize);
    }

}
