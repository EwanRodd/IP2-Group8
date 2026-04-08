using UnityEngine;
using System.Collections;

public class TopSpawner : MonoBehaviour
{
    public GameObject carPrefab;

    public int carsPerColumn = 5;
    public float spacing = 2f;

    public float spawnX = 10f;   // right side (off-screen)
    public float startY = 5f;    // top of column

    public float timeBetweenCars = 0.1f;
    public float timeBetweenWaves = 2f;

    public int columnCount;
    private int lastMissingIndex = -1;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < columnCount; i++)
        //while (true)
        {
            SpawnColumn();
            yield return new WaitForSeconds(timeBetweenWaves);
        }   
    }

    private void SpawnColumn()
    {
        int missingIndex;

        // Keep picking until it's different from last time
        do
        {
            missingIndex = Random.Range(0, carsPerColumn);
        }
        while (missingIndex == lastMissingIndex);

        lastMissingIndex = missingIndex; // store for next column

        for (int i = 0; i < carsPerColumn; i++)
        {
            if (i == missingIndex)
                continue;

            Vector3 spawnPos = new Vector3(
                spawnX,
                startY - (i * spacing),
                0f
            );

            Instantiate(carPrefab, spawnPos, Quaternion.identity);
        }
    }
}

