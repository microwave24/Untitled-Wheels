using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{

    public GameObject ShatteredObject;
    public float breakForce = 10f;



    public void BreakObject(float colliderVelocity, GameObject collider)
    {
        GameObject fracturedObject = Instantiate(ShatteredObject, transform.position, transform.rotation);

        for (int x = 0; x < fracturedObject.gameObject.transform.childCount; x++)
        {
            
            Vector3 force = (fracturedObject.transform.GetChild(x).transform.position - transform.position) * colliderVelocity/10;
            force += collider.GetComponent<Rigidbody>().velocity/2;
            //debug
            fracturedObject.transform.GetChild(x).GetComponent<ShatterRaw>().forceOfDirection = force;
            fracturedObject.transform.GetChild(x).GetComponent<ShatterRaw>().startPos = fracturedObject.transform.GetChild(x).transform.position;

            fracturedObject.transform.GetChild(x).GetComponent<Rigidbody>().velocity = force;

            fracturedObject.transform.GetChild(x);

            fracturedObject.transform.GetChild(x).parent = null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" | other.tag == "Enemy")
        {
            BreakObject(other.gameObject.GetComponent<Rigidbody>().velocity.magnitude, other.gameObject);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
