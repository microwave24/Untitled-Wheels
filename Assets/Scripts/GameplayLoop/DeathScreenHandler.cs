using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenHandler : MonoBehaviour
{
    public void restartLevel()
    {
        SceneManager.LoadScene(2);
        StaticGameVariables.playerHealth = 200f;
        StaticGameVariables.maxHealth = 200f;
        StaticGameVariables.currentWave = 1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
