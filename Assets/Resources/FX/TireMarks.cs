using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireMarks : MonoBehaviour
{
    RaycastHit GroundedCentre;

    public Transform body;
    void Update()
    {
        bool IsGrounded = Physics.Raycast(body.position, -body.up, out GroundedCentre, 1f);
        
        
        if (IsGrounded == true)
        {
            gameObject.GetComponent<TrailRenderer>().emitting = true;
        }
        else
        {
            gameObject.GetComponent<TrailRenderer>().emitting = false;
        }

    }
}
