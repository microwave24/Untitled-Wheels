using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour

{

    public float XCamOffset;
    public float YCamOffset;
    public float ZCamOffset;

    public float XRotCam;
    public float YRotCam;
    public float ZRotCam;



    public Transform Camera;

    public Camera mainCam;

    public Transform Object;

    public float turnSpeed = 0.1f;
    public float FamFollowSpeed = 1f;
    public float camDistoffset = 100f;
    public Transform camFollowPart;
    Quaternion rotGoal;
    Vector3 direction;


    private Vector3 prevCamPos;
    [SerializeField]
    private float mouseSens = 2f;
    [SerializeField]
    private float distfromobject = 120f;

    private float rotYvalue;


    // Update is called once per frame

    private void Update(){


        float mouseX;
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSens;
        }
        else
        {
           
            mouseX = 0;
        }
        rotYvalue += mouseX;
        transform.localEulerAngles = new Vector3(25, rotYvalue, 0) + gameObject.GetComponent<CameraShaker>().rotAddShake;
        transform.position = Object.position - transform.forward * distfromobject + gameObject.GetComponent<CameraShaker>().posAddShake;


    }

    void FixedUpdate()
    {



        //Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        ////if(Physics.Raycast(ray, out RaycastHit raycastHit)){
           // rotGoal = Quaternion.LookRotation(((Object.transform.position + raycastHit.point)/2 - transform.position).normalized);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
        //}

        //mainCam.transform.position = new Vector3(Mathf.Lerp(Object.transform.position.x, Object.transform.position.x, Time.deltaTime * turnSpeed), transform.position.y, Mathf.Lerp(Object.transform.position.z, Object.transform.position.z, Time.deltaTime * turnSpeed));
        //mainCam.transform.position = new Vector3(mainCam.transform.position.x + camDistoffset, mainCam.transform.position.y, mainCam.transform.position.z);

        






    }
}
