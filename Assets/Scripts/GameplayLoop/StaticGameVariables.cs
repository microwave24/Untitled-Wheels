using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticGameVariables
{
    public static GameObject player;

    public static int currentWave = 1;
    public static float playerHealth = 100;
    public static float maxHealth = 100;
    public static int cash = 10000000;
    public static int WeaponEquipped = 0;
    // 0 = gattling gun
    // 1 = Missiles
    // 2 = AA flak

    public static bool ownsMissile = false;
    public static bool ownsFlak = false;


    public static int ClipsSize = 60;
    public static int BulletsInClip = 60;
    public static float reloadTime = 2f;
    public static float Accuracy = 5f;
    public static float WeaponRadius = 2f;

    public static float gameTime = 0;
    public static float difficultyTimeMultiplier = 0;


    public static GameObject[] attachments = {};
    public static GameObject[] parts = {};
    public static GameObject[] enemies = {};

    public static List<int> PlayerParts = new List<int>();
    public static List<int> PlayerAttachments = new List<int>();

    public static Vector3 playerLocation;

    public static float SuspensionConstant = 0;
    public static float DampenConstant = 0;

    public static bool grounded = false;

}
