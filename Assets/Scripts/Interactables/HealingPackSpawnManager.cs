using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPackSpawnManager : MonoBehaviour {

    public float ySpawnHight;
    public float xSpawnWidth;

    public GameObject healingPacksPrefab;
    public List<HealingTime> spawnSchedule = new List<HealingTime>();

    private int scheduleIndex = 0;
    private int spawnIndex = 0;

    private HealingTime currentHT;
    private bool endOfInterval = true;
    private List<float> SpawnTimes = new List<float>();
    private List<GameObject> healingPacksPool = new List<GameObject>();
    private float startTime;

    private void Start(){
        currentHT = spawnSchedule[scheduleIndex];

        GameObject pack = Instantiate(healingPacksPrefab, new Vector3(Random.Range(-xSpawnWidth * 0.5f, xSpawnWidth * 0.5f), ySpawnHight, 0), Quaternion.identity);
        pack.transform.parent = transform;
        pack.gameObject.SetActive(false);
        healingPacksPool.Add(pack);
        startTime = Time.time;
    }

    private void Update(){
        if (scheduleIndex < spawnSchedule.Count){
            if (currentHT.startSec < Time.time - startTime && endOfInterval == true){
                endOfInterval = false;
                currentHT = spawnSchedule[scheduleIndex];
                SetSpawnTimes();
            }

            if (spawnIndex < SpawnTimes.Count){
                if (SpawnTimes.Count > 0 && SpawnTimes[spawnIndex] < Time.time - startTime){
                    for (int i = 0; i < healingPacksPool.Count; i++){
                        if (healingPacksPool[i].activeInHierarchy == false){
                            healingPacksPool[i].transform.position = new Vector3(Random.Range(-xSpawnWidth * 0.5f, xSpawnWidth * 0.5f), ySpawnHight, 0);
                            healingPacksPool[i].SetActive(true);
                            break;
                        }
                        else if (i == healingPacksPool.Count - 1 && healingPacksPool[i].activeInHierarchy){
                            GameObject pack = Instantiate(healingPacksPrefab, new Vector3(Random.Range(-xSpawnWidth * 0.5f, xSpawnWidth * 0.5f), ySpawnHight, 0), Quaternion.identity);
                            pack.transform.parent = transform;
                            healingPacksPool.Add(pack);
                            break;
                        }
                    }

                    spawnIndex++;
                }
            }

            if (currentHT.endSec <= Time.time - startTime && endOfInterval == false){
                endOfInterval = true;
                SpawnTimes.Clear();
                spawnIndex = 0;
                scheduleIndex++;
            }
        }

    }

    private void SetSpawnTimes(){
        for (int i = 0; i < currentHT.amountHealingSpawns; i++){
            float timeSlot = ((currentHT.endSec - currentHT.startSec) / (float)currentHT.amountHealingSpawns);
            float randomTime = Random.Range(0, timeSlot) + (timeSlot * i) + currentHT.startSec;
            SpawnTimes.Add(randomTime);
        }

    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(new Vector3(0, ySpawnHight, 0), new Vector3(xSpawnWidth, 1, 1));

    }

}

[System.Serializable]
public class HealingTime {
    public int startSec;
    public int endSec;
    public int amountHealingSpawns;

}
