using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    GameHandler gameHandler;

    public Slider HPValue;
    public Slider ShiftAbilityValue;
    public GameObject WaveCounter;

    [SerializeField]
    private RectTransform HealthBarFillTransform;
    [SerializeField]
    private RectTransform ShiftAbilityFillTransform;

    [SerializeField]
    private RectTransform AmmoText;

    [SerializeField]
    GameObject EnemyCounterTextTransform;


    [SerializeField]
    private Transform playerObject;


    // Start is called before the first frame update

    private void Awake()
    {
        gameHandler = GameObject.Find("Game").GetComponent<GameHandler>();
        WaveCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "wave: " + StaticGameVariables.currentWave.ToString();
    }

    void Start()
    {
    
    }

    void HealthBarFiller()
    {
        HPValue.value = Mathf.Lerp(HPValue.value, (StaticGameVariables.playerHealth / StaticGameVariables.maxHealth), 3f * Time.deltaTime);

    }

    public void ShiftAbilityfiller()
    {
        ShiftAbilityValue.value = Mathf.MoveTowards(ShiftAbilityValue.value, 1f, 1f/playerObject.GetComponent<PlayerController>().ShiftAbilityCooldown * Time.deltaTime); ;
    }
    // Update is called once per frame


    void Update()
    {

        AmmoText.GetComponent<TMPro.TextMeshProUGUI>().text = StaticGameVariables.BulletsInClip.ToString() + "/" + StaticGameVariables.ClipsSize.ToString();

        if (HPValue.value > 0.5)
        {
            HealthBarFillTransform.GetComponent<Image>().color = new Color((-2 * HPValue.value) + 2, 1f, 0f);
        }
        else
        {
            HealthBarFillTransform.GetComponent<Image>().color = new Color(1f, 2*HPValue.value, 0f);

        }

        if(ShiftAbilityValue.value > 0.95f)
        {
            ShiftAbilityFillTransform.GetComponent<Image>().color = new Color(0f, 0.1f * Mathf.Sin(5*Time.time) + 0.43f, 1f);
        }

        EnemyCounterTextTransform.GetComponent<TMPro.TextMeshProUGUI>().text = gameHandler.remainingEnemies.Count.ToString();
        HealthBarFiller();
        ShiftAbilityfiller();

        
        

    }
}
