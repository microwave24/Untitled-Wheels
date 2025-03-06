using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineNoise : MonoBehaviour
{

    public AudioSource Engine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;


        float engineSteepness = 0.2f;
        Engine.pitch = engineSteepness*(velocity / 20) + 0.66f;
    }
}
