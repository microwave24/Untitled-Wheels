using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarageHandler : MonoBehaviour
{


    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private Transform pedistol;

    [SerializeField]
    private Transform playerObject;

    // Start is called before the first frame update


    private float movePedistolWithCarZ = -3f;
    private float movePedistolWithCarX = 4f;

    private bool beginShift = false;


    private Vector3 CarStartPos;
    private Vector3 PedistolStartPos;

    private Vector3 targetCarPos;
    private Vector3 targetPedistol;
    [SerializeField]
    private Transform carTargetRot;
    [SerializeField]
    private GameObject modifyScreen;
    [SerializeField]
    private GameObject NextButton;
    [SerializeField]
    private GameObject ModifyButton;

    public GameObject Missiles;
    public GameObject Flak;
    public GameObject gatle;

    private bool ownsMissile = false;
    private bool ownsFlak = false;

    [SerializeField]
    private GameObject AApriceTag;
    [SerializeField]
    private GameObject MissilepriceTag;
    public GameObject cashDisplay;

    private void Awake()
    {
        CarStartPos = new Vector3(0, 0.5f, 0);
        PedistolStartPos = new Vector3(0, -10f, 0);

        targetCarPos = new Vector3(5f, playerObject.transform.position.y, -4f);
        targetPedistol = new Vector3(5f, pedistol.transform.position.y, -4f);

        if(StaticGameVariables.WeaponEquipped == 0)
        {
            gatle.SetActive(true);
            Missiles.SetActive(false);
            Flak.SetActive(false);
        }
        if (StaticGameVariables.WeaponEquipped == 1)
        {
            gatle.SetActive(false);
            Missiles.SetActive(true);
            Flak.SetActive(false);
        }
        if (StaticGameVariables.WeaponEquipped == 2)
        {
            gatle.SetActive(false);
            Missiles.SetActive(false);
            Flak.SetActive(true);
        }
    }


    void Start()
    {
    }
    private IEnumerator MoveCarAndPedistol()
    {
        float time = 0;
        float smoothness = 3f;
        while (time < 1)
        {
            playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, targetCarPos, Time.deltaTime );
            pedistol.transform.position = Vector3.Lerp(pedistol.transform.position, targetPedistol, Time.deltaTime);

            playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, carTargetRot.transform.rotation, Time.deltaTime);

            time += Time.deltaTime/smoothness;
            yield return null;
        }


    }
    public void EnterGarage()
    {
        
        ModifyButton.SetActive(false);
        NextButton.SetActive(false);

        modifyScreen.SetActive(true);
        StartCoroutine(MoveCarAndPedistol());
    }

    public void PlayGame()
    {
        //                      this number must be the index of the main stage scene
        SceneManager.LoadScene(2);
    }
    // Update is called once per frame
    void Update()
    {
        cashDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + StaticGameVariables.cash.ToString();

    }

    public void eqiup(int ID)
    {
        bool canEquip = false;
        if(ID == 0)
        {
            canEquip = true;
            StaticGameVariables.WeaponEquipped = 0;
            gatle.SetActive(true);
            Missiles.SetActive(false);
            Flak.SetActive(false);

        }
        if(ID == 1)
        {

            if(StaticGameVariables.ownsMissile == false)
            {
                if (StaticGameVariables.cash >= 6000)
                {
                    StaticGameVariables.cash -= 6000;
                    StaticGameVariables.WeaponEquipped = 1;
                    StaticGameVariables.ownsMissile = true;
                    canEquip = true;

                    gatle.SetActive(false);
                    Missiles.SetActive(true);
                    Flak.SetActive(false);


                    MissilepriceTag.GetComponent<TMPro.TextMeshProUGUI>().text = "owned";
                    MissilepriceTag.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0.65f, 0.65f, 0.65f);
                }
            }
            else
            {
                StaticGameVariables.WeaponEquipped = 1;
                gatle.SetActive(false);
                Missiles.SetActive(true);
                Flak.SetActive(false);

            }

            
        }
        if (ID == 2)
        {
            if (StaticGameVariables.ownsFlak == false)
            {
                if (StaticGameVariables.cash >= 6000)
                {
                    StaticGameVariables.cash -= 6000;
                    StaticGameVariables.WeaponEquipped = 2;
                    StaticGameVariables.ownsFlak = true;
                    canEquip = true;

                    gatle.SetActive(false);
                    Missiles.SetActive(false);
                    Flak.SetActive(true);

                    AApriceTag.GetComponent<TMPro.TextMeshProUGUI>().text = "owned";
                    AApriceTag.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0.65f, 0.65f, 0.65f);
                }
            }
            else
            {
                StaticGameVariables.WeaponEquipped = 2;
                gatle.SetActive(false);
                Missiles.SetActive(false);
                Flak.SetActive(true);

            }
        }

        
        

    }
}
