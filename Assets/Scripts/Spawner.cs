using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    // probabilities should sum to 100 but will still work regardless
    
    
    public GameObject enemyPrefab1;
    public int prefab1ProbabilityWeight = 20;
    public GameObject enemyPrefab2;
    public int prefab2ProbabilityWeight = 20;
    public GameObject enemyPrefab3;
    public int prefab3ProbabilityWeight = 20;
    public GameObject enemyPrefab4;
    public int prefab4ProbabilityWeight = 20;

    public GameObject[] bosses = new GameObject[4];



    public bool spawningAllowed = false;
    public float spawnRate = 2.0f;
    private float spawnTimer;
    private bool bossSpawned = false;


    private List<Tuple<GameObject, int>> enemies = new List<Tuple<GameObject, int>>();              // intial empty list of tuples: (enemyPrefab<>, integer weight)       
    private int totalWeight = 0;



    // Start is called before the first frame update
    void Start()
    {
        enemies.Add(new Tuple<GameObject, int>(enemyPrefab1, prefab1ProbabilityWeight));               // populate tuples list
        enemies.Add(new Tuple<GameObject, int>(enemyPrefab2, prefab2ProbabilityWeight));
        enemies.Add(new Tuple<GameObject, int>(enemyPrefab3, prefab3ProbabilityWeight));
        enemies.Add(new Tuple<GameObject, int>(enemyPrefab4, prefab4ProbabilityWeight));

        foreach (Tuple<GameObject, int> tuple in enemies) {
            totalWeight += tuple.Item2;
        }
    }



    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }



    private void SpawnEnemy()
    {
        if (spawningAllowed && Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 104 && Time.time > spawnTimer) {

            if (!bossSpawned && Vector3.Distance(GameObject.Find("Portal(Clone)").transform.position, GameManager.instance.player.transform.position) < 104)
            {
                int rand = UnityEngine.Random.Range(0, 4);
                Instantiate(bosses[rand], transform.position, transform.rotation);
                bossSpawned = true;
            }



            int randomNumber = UnityEngine.Random.Range(0, totalWeight);                                            // spawn random enemy from list based on probability weights
            int index = 0;

            foreach (Tuple<GameObject, int> tuple in enemies) {
                if (randomNumber <= tuple.Item2 + index) {
                    Instantiate(tuple.Item1, transform.position, transform.rotation);
                    spawnTimer = Time.time + spawnRate;
                    break;
                }

                else {
                    index += tuple.Item2;
                }
            }

        }
    }
}




