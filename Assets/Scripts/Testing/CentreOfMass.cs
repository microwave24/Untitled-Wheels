using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreOfMass : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 CentreOfMassPos;
    public bool Awake;
    public Rigidbody r;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        r.centerOfMass = CentreOfMassPos;
        r.WakeUp();
        Awake = !r.IsSleeping();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + transform.rotation * CentreOfMassPos, 1f);
    }
}
