using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
        float dist = Vector3.Distance(StaticGameVariables.playerLocation, transform.position);

        if (dist < 5)
        {
            StaticGameVariables.cash += Random.Range(50, 250);
            Destroy(gameObject);
        }
    }
}
