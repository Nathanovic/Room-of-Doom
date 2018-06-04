using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingManager : MonoBehaviour {

    public float ySpawnPos;

    public List<HealingTime> spawnSchedule = new List<HealingTime>();
    public List<GameObject> healingPacks = new List<GameObject>();



    private void Update(){
         


    }

}

[System.Serializable]
public class HealingTime {
    public int startSec;
    public int endSec;
    public int amountHealingSpawns;

}
