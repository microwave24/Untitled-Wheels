using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOfDeathMotion : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform targettedObject;

    public float rotateSpeed = 10f;
    public float breathingSize = 0.5f;
    public float breathingSpeed = 1f;

    private float targetTime = 0f;
    private float distFromCentre = 0f;
    private float arena_rad = 300f;

    public float fadeSpeed = 1f;
    private IEnumerator shrink()
    {
        while (targetTime > -1f)
        {
            float alphaColor = transform.GetComponent<SpriteRenderer>().material.color.a;


            if (distFromCentre > arena_rad)
            {
                
                targetTime += 0.01f;
                transform.localScale = new Vector3(Mathf.Pow(0.48f, (targetTime - 2.5f)) + 2, Mathf.Pow(0.48f, (targetTime - 2.5f)) + 2, 1);
 

                if (alphaColor >= 1)
                {
                    alphaColor = 1f;
                }
                else
                {
                    transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alphaColor + 0.01f);

                }
            }
            else
            {
                targetTime = 0f;
                

                if (alphaColor <= 0.01)
                {
                    alphaColor = 0.01f;
                }
                else
                {
                    transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alphaColor - 0.01f);
                }
            }



            yield return new WaitForSeconds(Time.deltaTime);

        }

        
        

    }

    void Start()
    {
        transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.0f);
        StartCoroutine(shrink());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(targettedObject.position.x, -0.45f, targettedObject.position.z);
        Vector3 centre = new Vector3(0, -0.6f, 0);
        distFromCentre = Vector3.Distance(centre, transform.position);
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);

        

        
        //transform.localScale = new Vector3(breathingSize * (Mathf.Sin(breathingSpeed * Time.time) + 4), breathingSize * (Mathf.Sin(breathingSpeed * Time.time) + 4), 1);
    }
}
