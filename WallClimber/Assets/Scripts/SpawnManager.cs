using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject[] obstaclePrefab;
    public float spawnDistance;
    private GameObject[] walls = new GameObject[2];
    public GameObject[] enemiesPrefabs;
    private Vector3 spawnPos = new Vector3(-5f,15f,0);
    
    private Player playerScript;
    private GameObject playerRb;
    // Start is called before the first frame update
    void Start() {
        walls[0] = Instantiate(obstaclePrefab[0],  new Vector3(-5, 30, 0), obstaclePrefab[0].transform.rotation);
        walls[1] = Instantiate(obstaclePrefab[0], new Vector3(5, 30, 0), Quaternion.Euler(0, 180, 0));
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        playerRb = GameObject.Find("Player");

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!playerScript.gameOver) {
            if (walls[0].transform.position.y - playerRb.transform.position.y < 10) {
                SpawnObstacle();
                SpawnEnemies();
            }
        }
    }

    void SpawnObstacle() {
        int randomIndex = Random.Range(0, obstaclePrefab.Length/2);
        walls[0] = Instantiate(obstaclePrefab[randomIndex*2],  walls[0].transform.position + new Vector3(0, 15, 0), Quaternion.Euler(0, 0, 0));
        walls[1] = Instantiate(obstaclePrefab[randomIndex*2+1], walls[1].transform.position + new Vector3(0, 15, 0), Quaternion.Euler(0, 180, 0));
    }
    
    void SpawnEnemies() {
        int randomIndex = Random.Range(0, enemiesPrefabs.Length);
        float spawnPositionY = Random.Range(-6, +6);
        float spawnPositionX = 0;
        if (randomIndex == 0) {
            spawnPositionX = Random.Range(-3, 3);
            Instantiate(enemiesPrefabs[randomIndex],   new Vector3(spawnPositionX, walls[0].transform.position.y + spawnPositionY, 0), Quaternion.Euler(0,0,0));

        } else if(randomIndex == 1){
            spawnPositionX = Random.Range(0, 2) * 6 - 3;
            if (spawnPositionX < 0) {
                Instantiate(enemiesPrefabs[randomIndex],   new Vector3(spawnPositionX-0.3f, walls[0].transform.position.y + spawnPositionY, 0), Quaternion.Euler(0,180,0));
            }
            else {
                Instantiate(enemiesPrefabs[randomIndex],   new Vector3(spawnPositionX+0.3f, walls[0].transform.position.y + spawnPositionY, 0), Quaternion.Euler(0,0,0));
            }
        }
    }
}
