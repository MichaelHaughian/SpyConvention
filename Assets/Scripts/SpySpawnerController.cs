using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpySpawnerController : MonoBehaviour
{
    int spawnPointsUsed;
    int spiesCreated;

    [SerializeField] GameObject Spy1;
    [SerializeField] GameObject Spy2;
    [SerializeField] GameObject Spy3;
    [SerializeField] GameObject Spy4;
    [SerializeField] GameObject Spy5;
    [SerializeField] GameObject Spy6;
    [SerializeField] GameObject Spy7;
    [SerializeField] GameObject Spy8;

    GameObject[] spawnPoints;

    [SerializeField] GameObject SpawnPoint1;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] GameObject SpawnPoint3;
    [SerializeField] GameObject SpawnPoint4;
    [SerializeField] GameObject SpawnPoint5;
    [SerializeField] GameObject SpawnPoint6;
    [SerializeField] GameObject SpawnPoint7;
    [SerializeField] GameObject SpawnPoint8;

    void Awake() {
        spawnPointsUsed = 0;
        spiesCreated = 0;

        GameObject[] spies = new GameObject[] { Spy1, Spy2, Spy3, Spy4, Spy5, Spy6, Spy7, Spy8 };
        spawnPoints = new GameObject[] { SpawnPoint1, SpawnPoint2, SpawnPoint3, SpawnPoint4,
            SpawnPoint5, SpawnPoint6, SpawnPoint7, SpawnPoint8 };

        System.Random spiesRandomOrder = new System.Random();
        System.Random spawnsRandomOrder = new System.Random();

        spies = spies.OrderBy(x => spiesRandomOrder.Next()).ToArray();
        spawnPoints = spawnPoints.OrderBy(x => spawnsRandomOrder.Next()).ToArray();


        foreach (GameObject spy in spies) {
            SpawnSpy(spies[spiesCreated]);
        }
    }

    void SpawnSpy(GameObject spy) {
        GameObject spawnLocation;

        spawnLocation = spawnPoints[spawnPointsUsed];       

        if(spiesCreated == 0) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isSpy = false;
        }

        if(spiesCreated == 1) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheInjuredOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 2) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheOldOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 3) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheQuietOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 4) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheSelfishOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 5) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheMysogynistOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 6) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheClassyOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        if (spiesCreated == 7) {
            GameObject spawnedSpy = Instantiate(spy, new Vector2(spawnLocation.transform.position.x, spawnLocation.transform.position.y), Quaternion.identity);
            spawnedSpy.GetComponent<SpyController>().isTheBoringOne = true;
            spawnedSpy.GetComponent<SpyController>().isSpy = true;
        }

        spiesCreated++;
        spawnPointsUsed++;
    }
}

