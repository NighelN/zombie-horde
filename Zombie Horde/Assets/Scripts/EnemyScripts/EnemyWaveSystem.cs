using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyObject enemyObject;
        public float chance = 0;
        public int spawnAfterDays = 0;
    }

    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private bool hasSpawned = false;
    [SerializeField] private EnemySpawnData[] enemySpawnDatas;

    [SerializeField] private int startingAmount = 2;
    [SerializeField] private int zombiesPerDay = 2;

    private int spawnDay = 0;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle.GetComponent<DayNightCycle>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the night is in progress
        if (dayNightCycle.IsNight())
        {
            //Checks if it has spawned the zombies
            if(!hasSpawned){
                //Checks if its currently spawning
                if (!isSpawning)
                {
                    Debug.Log("start wave");
                    isSpawning = true;
                    spawnDay = dayNightCycle.daysPassed;
                    StartCoroutine(SpawningEnemies());
                }
            }
        }
        if (!dayNightCycle.IsNight())
        {
            hasSpawned = false;
        }
    }

    // IEnumerator for spawning the enemies, will wait 2 seconds and call the fuction EnemySpawning() 
    IEnumerator SpawningEnemies()
    {
        hasSpawned = true;
        //Loops though all the enemies it can spawn
        for (int i = 0; i < startingAmount + zombiesPerDay * spawnDay; i++)
        {
            SpawnEnemy();
            //Adds a delay to the spawning of the enemy
            yield return new WaitForSeconds(dayNightCycle.dayNightCycleMin * (1f / 24f * ((24 - dayNightCycle.startNight) + dayNightCycle.endNight)) * 60f / (startingAmount + zombiesPerDay * spawnDay));
        }
        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        //The random chance of spawning an enemy
        float chance = Random.Range(0, 1000);
        float currentChance = 0;

        //Loops though all the enemy spawn data
        foreach (var enemySpawnData in enemySpawnDatas)
        {
            //Checks if days passed is higher then the required days
            if (spawnDay >= enemySpawnData.spawnAfterDays)
            {
                //Checks if the random chance is lower then the change * 10 + current chance
                if (chance < enemySpawnData.chance * 10 + currentChance)
                {
                    enemySpawner.EnemySpawning(enemySpawnData.enemyObject, dayNightCycle);
                    return;
                }
                //Else increase the chance
                else
                {
                    currentChance += enemySpawnData.chance * 10;
                }
            }
        }

        enemySpawner.EnemySpawning(enemySpawnDatas[0].enemyObject, dayNightCycle);
    }
}
