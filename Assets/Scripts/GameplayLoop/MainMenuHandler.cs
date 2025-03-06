using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private float CameraWobbleStrength = 0.5f;
    [SerializeField]
    private float CameraWobbleSpeed = 1f;


    public void PlayGame()
    {
        //                      this number must be the index of the main stage scene
        SceneManager.LoadScene(2);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        mainCam.transform.rotation = Quaternion.Euler(CameraWobbleStrength * Mathf.Sin(CameraWobbleSpeed * Time.time), -CameraWobbleStrength * Mathf.Sin(CameraWobbleSpeed * Time.time), CameraWobbleStrength * Mathf.Sin(CameraWobbleSpeed * Time.time));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
