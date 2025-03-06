using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{

    public Rigidbody bodyRB;

    //public Transform WheelModel;

    public Transform SuspensionAnchorPoint;

    public float SuspensionHeight;
    public Transform SuspensionEndpoint;

    private float SuspensionOffset;
    public LayerMask LayerToIgnore;

    private float previousPosition;

    public GameObject wheelModel;

    // Start is called before the first frame update
    private void Start()
    {

        previousPosition = SuspensionEndpoint.localPosition.y;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        RaycastHit SuspensionRay;


        Ray ray = new Ray(SuspensionAnchorPoint.position, -transform.up);
        Physics.Raycast(ray, out SuspensionRay, SuspensionHeight, ~LayerToIgnore);

        Debug.DrawRay(SuspensionAnchorPoint.position, -transform.up * SuspensionHeight, Color.green);


        float velocity = (SuspensionEndpoint.localPosition.y - previousPosition) / Time.fixedDeltaTime;
        float hitPointDistance;


        if (SuspensionRay.collider != null)
        {
            StaticGameVariables.grounded = true;


            hitPointDistance = Vector3.Distance(SuspensionAnchorPoint.position, SuspensionRay.point);
            SuspensionOffset = SuspensionHeight - hitPointDistance;


            float forceStrength = (StaticGameVariables.SuspensionConstant * SuspensionOffset) + (StaticGameVariables.DampenConstant * velocity);

            bodyRB.AddForceAtPosition(forceStrength * SuspensionAnchorPoint.transform.up, SuspensionAnchorPoint.position);
            Debug.DrawRay(SuspensionAnchorPoint.position, SuspensionAnchorPoint.up * forceStrength * 0.000003f, Color.magenta);




        }
        else
        {
            StaticGameVariables.grounded = false;
            hitPointDistance = SuspensionHeight;
            SuspensionOffset = 0;
        }

        previousPosition = SuspensionEndpoint.localPosition.y;

        if (SuspensionRay.collider != null)
        {
            SuspensionEndpoint.position = SuspensionRay.point;
        }
        else
        {
            SuspensionEndpoint.position = SuspensionAnchorPoint.position + (-transform.up * SuspensionHeight);

        }
        //wheelModel.transform.position = SuspensionEndpoint.position;







    }
}
