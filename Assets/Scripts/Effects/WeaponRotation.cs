using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Camera mainCam;
    public Transform Weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray AimAT = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(AimAT, out RaycastHit AimHit))
        {
            Weapon.transform.LookAt(AimHit.point);
        }

        Quaternion LookTarget = Quaternion.LookRotation(AimHit.point - Weapon.transform.position);
        Weapon.transform.rotation = Quaternion.Slerp(Weapon.transform.rotation, LookTarget, Time.deltaTime);

    }
}
