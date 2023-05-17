using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [System.Serializable]
    public struct EnemiesProbabilitiesEntry
    {
        public int level;
        public float[] probabilities;
    }

    [System.Serializable]
    public struct SpawnIntervalEntry
    {
        public int level;
        public SpawnInterval interval;
    }

    [System.Serializable]
    public struct SpawnInterval
    {
        public float lowInterval;
        public float highInterval;
    }

    public ObjectPooling[] enemyPoolings;
    public ObjectPooling[] fishPoolings;
    public float initialSpawnFishInterval = 2f;
    public float spawnFishIntervalIncrease = 0.1f;
    public float initialSpawnEnemyInterval = 5f;
    public float spawnEnemyIntervalIncrease = 0.1f;
    public EnemiesProbabilitiesEntry[] enemiesProbabilitiesByLevel;
    public SpawnIntervalEntry[] spawnFishIntervalByLevel;
    public SpawnIntervalEntry[] spawnEnemyIntervalByLevel;

    private GameObject[] spawnBoundaries;
    private float spawnFishDelay = 0.5f;
    private float spawnEnemyDelay = 5f;
    private Dictionary<int, float[]> enemiesProbabilitiesByLevelDict = new();
    private float[] enemiesProbabilities;
    private Dictionary<int, SpawnInterval> fishSpawnIntervalByLevelDict = new();
    private SpawnInterval fishSpawnInterval;
    private Dictionary<int, SpawnInterval> enemySpawnIntervalByLevelDict = new();
    private SpawnInterval enemySpawnInterval;

    // Start is called before the first frame update
    void Start()
    {

        foreach (EnemiesProbabilitiesEntry entry in enemiesProbabilitiesByLevel)
        {
            enemiesProbabilitiesByLevelDict.Add(entry.level, entry.probabilities);
        }

        foreach (SpawnIntervalEntry entry in spawnFishIntervalByLevel)
        {
            fishSpawnIntervalByLevelDict.Add(entry.level, entry.interval);
        }

        foreach (SpawnIntervalEntry entry in spawnEnemyIntervalByLevel)
        {
            enemySpawnIntervalByLevelDict.Add(entry.level, entry.interval);
        }

        spawnBoundaries = new GameObject[transform.GetChild(0).childCount];
        for(int i = 0; i < transform.GetChild(0).childCount; i++) {
            spawnBoundaries[i] = transform.GetChild(0).GetChild(i).gameObject;
        }

        SetSpawnIntervalVariablesByLevel(1);
        SetEnemiesProbabilitiesByLevel(1);

        StartCoroutine(RepeatSpawnEnemy());
        StartCoroutine(RepeatSpawnFish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        GameObject enemy = GetRandomObjectWithProbs(enemyPoolings, enemiesProbabilities);
        enemy.transform.position = GetRandomSpawnPosition();
        enemy.GetComponent<MovementController>().OnCreate();
    }

    void SpawnFish()
    {
        GameObject fish = fishPoolings[Random.Range(0, fishPoolings.Length)].GetObject();
        fish.transform.position = GetRandomSpawnPosition();
        fish.GetComponent<MovementController>().OnCreate();
    }

    IEnumerator RepeatSpawnFish()
    {
        yield return new WaitForSeconds(spawnFishDelay);
        while(true)
        {
            SpawnFish();
            float waitForSpawn = Random.Range(fishSpawnInterval.lowInterval, fishSpawnInterval.highInterval);
            yield return new WaitForSeconds(waitForSpawn);
        }
    }

    IEnumerator RepeatSpawnEnemy()
    {
        yield return new WaitForSeconds(spawnEnemyDelay);
        while (true)
        {
            SpawnEnemy();
            float waitForSpawn = Random.Range(enemySpawnInterval.lowInterval, enemySpawnInterval.highInterval);
            yield return new WaitForSeconds(waitForSpawn);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        int edge = Random.Range(0, spawnBoundaries.Length);
        Vector3 topBoundary = spawnBoundaries[edge].transform.position;
        Vector3 botBoundary = spawnBoundaries[(edge + 1) % spawnBoundaries.Length].transform.position;
        Vector3 slope = botBoundary - topBoundary;

        float shift = Random.Range(0f, 1f);
        return topBoundary + slope * shift;
    }

    public void SetSpawnIntervalVariablesByLevel(int level)
    {
        if(fishSpawnIntervalByLevelDict.ContainsKey(level))
        {
            fishSpawnInterval = fishSpawnIntervalByLevelDict[level];
        }

        if (enemySpawnIntervalByLevelDict.ContainsKey(level))
        {
            enemySpawnInterval = enemySpawnIntervalByLevelDict[level];
        }
    }

    GameObject GetRandomObjectWithProbs(ObjectPooling[] objectPoolings, float[] probabilities)
    {
        float prob = Random.Range(0, 1f);
        float lowBound = 0;
        float highBound = probabilities[0];

        for(int i = 0; i < objectPoolings.Length; i++)
        {
            if (prob >= lowBound && prob <= highBound)
            {
                return objectPoolings[i].GetObject();
            }

            lowBound = probabilities[i];
            highBound = probabilities[i + 1];
        }

        return null;
    }

    public void SetEnemiesProbabilitiesByLevel(int level)
    {
        if(enemiesProbabilitiesByLevelDict.ContainsKey(level))
        {
            enemiesProbabilities = enemiesProbabilitiesByLevelDict[level];
        }
    }
}
