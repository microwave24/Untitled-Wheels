using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class WaveCubeSplashDown : MonoBehaviour
{
    public GameObject landingFx;
    public TrailRenderer SpawnTrail;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaveCube());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private IEnumerator SpawnWaveCube()
    {
        yield return new WaitForSeconds(1); ;
        Vector3 startPos = new Vector3(0, 500, 0);
        Vector3 endPos = new Vector3(0, 7.7f, 0);
        float time = 0f;

        while (time < 10f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, time / 0.1f);
            time += Time.deltaTime;
            yield return null;
            if (transform.position == endPos)
            {
                break;
            }
           

        }
        transform.position = endPos;
        CameraShaker.Instance.ShakeOnce(8, 8, 0.1f, 1f);
        var fx = Instantiate(landingFx);
        fx.transform.position = gameObject.transform.position - new Vector3 (0,1,0);

        // cam effects


    }
}
