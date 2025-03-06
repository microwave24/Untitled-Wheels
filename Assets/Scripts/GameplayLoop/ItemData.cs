using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public bool isPart;

    public int rarity = 0;

    //--------------------------------------------POSITIVE STATS
    // damage based stats
    public float DamageMultiplier = 1;
    public float AddDamage = 0f;

    //weapon based stats

    public float FireRateDiminsher = 1f;
    public float ReloadSpeedDiminsher = 1f;
    public float bulletSpeedMulitplier = 1f;
    public float weaponAccuracyMultiplier = 1f;


    // health based stats
    public bool DoesRegen;
    public float HealthMutliplier = 1f;
    public float AddMaxHealth = 0f;


    // movement based stats

    public float speedMultiplier = 1f;
    public float speedAdder = 0f;

    //passive abilities

    //-------------------------------------------NEGETIVE STATS

}
