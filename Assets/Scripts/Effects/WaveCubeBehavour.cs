using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCubeBehavour : MonoBehaviour
{
    public GameObject game;
    public float health = 10f;
    private bool executed = false;





    private void Start()
    {
        StartCoroutine(shatter());

        executed = true;
        GameObject.Find("Game").GetComponent<GameHandler>().CalculateWave();
        for (int x = 0; x < gameObject.gameObject.transform.childCount; x++)
        {
            gameObject.transform.GetChild(x).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        }
        StartCoroutine(shatter());
    }

    private IEnumerator shatter()
    {


        for (int x = 0; x < gameObject.gameObject.transform.childCount; x++)
        {
            gameObject.transform.GetChild(x).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            
            gameObject.transform.GetChild(x).GetComponent<Rigidbody>().velocity = Random.onUnitSphere * 500;

        }
        yield return new WaitForSeconds(1f);
        for (int x = 0; x < gameObject.gameObject.transform.childCount; x++)
        {

            gameObject.transform.GetChild(x).GetComponent<MeshCollider>().enabled = false;

        }

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
