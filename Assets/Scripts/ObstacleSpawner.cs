using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Obstacle[] obstacles;
    public float deleteTimer = 5f;
    public float spawnWaitTime = 3f;
    private float currentTimer = 0f;

    private List<GameObject> spawned = new List<GameObject>();
    private Queue<Obstacle> spawnQueue = new Queue<Obstacle>();


    private void Update() {
        Spawn();
        TimerUpdate();
    }

    void Spawn() {
        bool generate = Random.Range(0, 5) == 1;
        if(generate && currentTimer >= spawnWaitTime) {
            float weightSum = 0f;
            foreach(Obstacle o in obstacles) {
                weightSum += o.spawnWeight;
            }

            float weight = Random.Range(0, weightSum);
            foreach(Obstacle o in obstacles) {
                if(o.spawnWeight > weight) {
                    spawnQueue.Enqueue(o);
                    currentTimer = 0f;
                    break;
                }
            }
        }


        if(spawnQueue.Count > 0) {
            Obstacle nextSpawn = spawnQueue.Dequeue();
            float xPos = Random.Range(nextSpawn.minimumSpawnRange.x, nextSpawn.maximumSpawnRange.x);
            float yPos = Random.Range(nextSpawn.minimumSpawnRange.y, nextSpawn.maximumSpawnRange.y);
            float zPos = Random.Range(nextSpawn.minimumSpawnRange.z, nextSpawn.maximumSpawnRange.z);

            GameObject spawnedObj = (GameObject)PrefabUtility.InstantiatePrefab(nextSpawn.prefab, transform);
            spawnedObj.transform.position = new Vector3(xPos, yPos, zPos);
            spawnedObj.transform.rotation = nextSpawn.prefab.transform.rotation;
            spawnedObj.transform.Rotate(Vector3.up, Random.Range(nextSpawn.randomRotationRange.x, nextSpawn.randomRotationRange.y));
            spawnedObj.transform.localScale = new Vector3(Random.Range(nextSpawn.minimumScale.x, nextSpawn.maximumScale.x),
                Random.Range(nextSpawn.minimumScale.y, nextSpawn.maximumScale.y),
                Random.Range(nextSpawn.minimumScale.z, nextSpawn.maximumScale.z));

            spawned.Add(spawnedObj);
            Destroy(spawnedObj, deleteTimer);
        }
    }

    void TimerUpdate() {
        if(currentTimer < spawnWaitTime) {
            currentTimer += Time.deltaTime;
        } else if (currentTimer > spawnWaitTime) {
            currentTimer = spawnWaitTime;
        }
    }
}


[System.Serializable]
public struct Obstacle {
    public GameObject prefab;
    [Range(0, 1)]
    public float spawnWeight;

    public Vector3 minimumSpawnRange;
    public Vector3 maximumSpawnRange;

    public Vector2 randomRotationRange;

    public Vector3 minimumScale;
    public Vector3 maximumScale;
}
