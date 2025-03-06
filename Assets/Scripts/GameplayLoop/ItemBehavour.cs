using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavour : MonoBehaviour
{
    public bool isPart= true;
    public int PartID = 0;
    public int AttachID = 0;
    public GameObject player;
    // Start is called before the first frame update
    public float BobbingAmplify = 0.0051f;
    public float BobbingSpeed = 3f;



    private void Start()
    {
        transform.position = transform.position + new Vector3(0, 5, 0);
    }


    private void Update()
    {
        transform.position = new Vector3(transform.position.x, (2.5f + (BobbingAmplify * Mathf.Sin(BobbingSpeed * Time.time))), transform.position.z);
        transform.Rotate(30 * Time.deltaTime, 30 * Time.deltaTime, 30 * Time.deltaTime);

       
        float distFromPlayer = Vector3.Distance(StaticGameVariables.playerLocation, transform.position);

        if(distFromPlayer < 35)
        {
            transform.position = new Vector3(

                Mathf.Lerp(transform.position.x, StaticGameVariables.playerLocation.x, Time.deltaTime * 7f),
                transform.position.y,
                Mathf.Lerp(transform.position.z, StaticGameVariables.playerLocation.z, Time.deltaTime * 7f)


                );

            if(distFromPlayer < 6)
            {
                if(isPart == true)
                {
                    StaticGameVariables.PlayerParts.Add(PartID);


                    // stat modify

                    //gameObject.GetComponent<ItemData>()

                    StaticGameVariables.player.GetComponent<PlayerController>().damage = StaticGameVariables.player.GetComponent<PlayerController>().damage * gameObject.GetComponent<ItemData>().DamageMultiplier;
                    StaticGameVariables.player.GetComponent<PlayerController>().damage = StaticGameVariables.player.GetComponent<PlayerController>().damage + gameObject.GetComponent<ItemData>().AddDamage;
                    StaticGameVariables.player.GetComponent<PlayerController>().fireRate *= gameObject.GetComponent<ItemData>().FireRateDiminsher;
                    StaticGameVariables.reloadTime *= gameObject.GetComponent<ItemData>().ReloadSpeedDiminsher;
                    StaticGameVariables.player.GetComponent<PlayerController>().BulletTrail.GetComponent<TrailRenderer>().time *= gameObject.GetComponent<ItemData>().bulletSpeedMulitplier;
                    StaticGameVariables.reloadTime *= gameObject.GetComponent<ItemData>().ReloadSpeedDiminsher;


                    
                    
                    StaticGameVariables.maxHealth += gameObject.GetComponent<ItemData>().AddMaxHealth;
                    StaticGameVariables.maxHealth *= gameObject.GetComponent<ItemData>().HealthMutliplier;


                    Destroy(gameObject);
                }

                
            }
        }
    }
}
