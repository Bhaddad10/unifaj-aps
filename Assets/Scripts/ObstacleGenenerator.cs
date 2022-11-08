using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenenerator : MonoBehaviour
{
    public float distance = 20f;
    public float objectArea = 10f;
    public float safeArea = 10f;

    public float offset = 20f;

    float[] lanes = { -6.5f, 0f, 6.5f };

    public List<GameObject> obstacles;
    public GameObject jumpCoins;
    public GameObject coins;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 generateFrom = gameObject.transform.position;
            gameObject.transform.position += Vector3.forward * (distance - objectArea);
            GenerateObstacles(generateFrom);
        }
    }

    private void GenerateObstacles(Vector3 generateFrom)
    {
        for (float zObstacle = generateFrom.z + objectArea; zObstacle <= generateFrom.z + distance + objectArea; zObstacle += objectArea)
        {
            int lane1 = Random.Range(0, 3);

            int lane2 = Random.Range(0, 3);
            while (lane2 == lane1)
            {
                lane2 = Random.Range(0, 3);
            }

            GenerateObstacle(lane1, zObstacle);
            GenerateObstacle(lane2, zObstacle);
        }
    }

    private void GenerateObstacle(int lane, float zObstacle)
    {
        Vector3 p = new Vector3(lanes[lane], 0, zObstacle);

        

        GameObject obstacleToSpawn = obstacles[Random.Range(0, obstacles.Count)];
        GameObject obstacle = Instantiate(obstacleToSpawn);

        //GenerateCoins(obstacle);

        obstacle.transform.position = p;
    }
}
