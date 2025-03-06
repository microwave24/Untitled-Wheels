using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    ItemBehavour itembehav;

    // pick random item from part array - DONE
    // random location around the enem - DONE
    // Spawn item
    // lerp item --from centre of enemy to location
    // have the scale lerp aswell
    // when player gets close, the part moves towards player
    private Vector3 DeathPosition;
    private Vector3 NewPosition;

    private float TimeTaken = 0f;
    private float LerpSpeed = 6.5f;

    private GameObject spawnedPart;
    private GameObject spawnedMoney;
    private int SelectedPart;
    ItemHandler itemHandler;

    public GameObject Money;

    public int CommonChance = 50;
    public int UncommonChance = 30;
    public int RareChance = 15;
    public int EpicChance = 5;
    

    private void Start()
    {





        SelectedPart = Random.Range(0, StaticGameVariables.parts.Length);
        float randangle = Random.Range(-1f, 1f) * Mathf.PI * 2;


        // gets a random gerenral spawning area for the enemies
        float spawnCricleAreaX = 10 * Mathf.Cos(randangle);
        float spawnCricleAreaZ = 10 * Mathf.Sin(randangle);
        Vector3 SpawnCricleArea = new Vector3(spawnCricleAreaX, 1.5f, spawnCricleAreaZ);


        DeathPosition = transform.position;

        NewPosition = transform.position + SpawnCricleArea;

        spawnedPart = Instantiate(StaticGameVariables.parts[SelectedPart]);
        spawnedPart.transform.position = transform.position;


        spawnedPart.GetComponent<ItemBehavour>().isPart = true;
        spawnedPart.GetComponent<ItemBehavour>().PartID = SelectedPart;


        spawnedMoney = Instantiate(Money);

        spawnedMoney.transform.position = transform.position;


    }

    private void Update()
    {

        if(spawnedPart != null)
        {
            float distFromStart = Vector3.Distance(transform.position, spawnedPart.transform.position);
            if (distFromStart < 9.9)
            {
                spawnedPart.transform.position = new Vector3(
                Mathf.Lerp(spawnedPart.transform.position.x, NewPosition.x, Time.deltaTime * LerpSpeed),
                transform.position.y,
                Mathf.Lerp(spawnedPart.transform.position.z, NewPosition.z, Time.deltaTime * LerpSpeed)
                );

            }
            else
            {

            }
        }
        if(spawnedMoney != null)
        {
            spawnedMoney.transform.Rotate(0, 50 * Time.deltaTime, 0);
            float distFromStart = Vector3.Distance(transform.position, spawnedMoney.transform.position);
            if (distFromStart < 9.9)
            {
                spawnedMoney.transform.position = new Vector3(
                Mathf.Lerp(spawnedMoney.transform.position.x, NewPosition.x, Time.deltaTime * LerpSpeed),
                transform.position.y,
                Mathf.Lerp(spawnedMoney.transform.position.z, NewPosition.z, Time.deltaTime * LerpSpeed)
                ) + new Vector3(Random.Range(-3, 3), -0.6f, Random.Range(-3, 3));

                

            }

        }

        

        



        //spawnedPart.transform.position = Vector3.Lerp(spawnedPart.transform.position, NewPosition, Time.deltaTime * LerpSpeed);

    }


}
