using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    private void Start()
    {
        
        StaticGameVariables.parts = Resources.LoadAll<GameObject>("Objects/ItemDrops");
        StaticGameVariables.enemies = Resources.LoadAll<GameObject>("Enemies");
    }
}
