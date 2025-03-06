using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public float health = 100f;
    public float MaxHealth;
    public bool playerDead = false;



    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private RectTransform DeadText;
    [SerializeField]
    private RectTransform ResetButton;
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private GameObject ShiftAbility;


    [SerializeField]
    private GameObject PostProcessingHandler;


    public void TakeDamage(float Amount)
    {
        StaticGameVariables.playerHealth -= Amount;
    }

    private void Start()
    {
        StaticGameVariables.player = gameObject;
    }
    private void playDeathScreen()
    {

        if (deathScreen.active == false)
        {
            //healthbar
            HealthBar.SetActive(false);

            //shiftability
            ShiftAbility.SetActive(false);

            //deadscreen enabler
            deathScreen.SetActive(true);
            DeadText.gameObject.SetActive(true);
            ResetButton.gameObject.SetActive(true);
        }

            
    }

    // Update is called once per frame
    void Update()
    {
        



        StaticGameVariables.playerLocation = transform.position;
        if (StaticGameVariables.playerHealth <= 0)
        {
            playerDead = true;
            

            if (deathScreen.active == false)
            {
                StaticGameVariables.currentWave = 0;
                //StartCoroutine(PostProcessingHandler.GetComponent<PostProcessingHandler>().deathColor());
            }
            playDeathScreen();
        }


    }
}
