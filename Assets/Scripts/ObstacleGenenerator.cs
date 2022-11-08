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

    public List<Obstacle> obstacles;
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


        Obstacle obstacleProperties = obstacles[Random.Range(0, obstacles.Count)];
        GameObject obstacleToSpawn = obstacleProperties.obstacle;
        GameObject obstacle = Instantiate(obstacleToSpawn);
        obstacle.transform.position = p;

        if (obstacleProperties.allowCoins && obstacleProperties.allowJumpCoins)
        {
            int result = Random.Range(0, 3);
            switch (result)
            {
                case 0:
                    InstObjectAtPos(coins, p);
                    break;
                case 1:
                    InstObjectAtPos(jumpCoins, p);
                    break;
            }
        } else if (obstacleProperties.allowCoins) {
            if (Random.value < 0.5f)
                InstObjectAtPos(coins, p);
        } else if (obstacleProperties.allowJumpCoins) {
            if (Random.value < 0.5f)
                InstObjectAtPos(jumpCoins, p);
        }


    }

    private void InstObjectAtPos(GameObject prefab, Vector3 pos)
    {
        GameObject obstacle = Instantiate(prefab);
        obstacle.transform.position = pos;
    }

    [Serializable]
    public class Obstacle
    {
        public GameObject obstacle;
        public bool allowCoins;
        public bool allowJumpCoins;
    }
}
