using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Generates random objects procedurally
public class ObstacleGenenerator : MonoBehaviour
{
    // Editor parameters
    public float distance = 20f;
    public float objectArea = 10f;
    public float safeArea = 10f;

    public float offset = 20f;

    // Lanes x-position
    float[] lanes = { -6.5f, 0f, 6.5f };

    // Obstacles and coins used by the generator
    public List<Obstacle> obstacles;
    public GameObject jumpCoins;
    public GameObject coins;

    private void OnTriggerEnter(Collider other)
    {
        //When player collides, generate obstacles, and move the next trigger forward
        if (other.CompareTag("Player"))
        {
            Vector3 generateFrom = gameObject.transform.position;
            gameObject.transform.position += Vector3.forward * (distance + objectArea);
            GenerateObstacles(generateFrom);
        }
    }

    private void GenerateObstacles(Vector3 generateFrom)
    {
        // for each step
        for (float zObstacle = generateFrom.z + objectArea; zObstacle <= generateFrom.z + distance + objectArea; zObstacle += objectArea)
        {
            // decide which lanes are going to have obstacles
            int lane1 = Random.Range(0, 3);
            int lane2 = Random.Range(0, 3);
            while (lane2 == lane1)
            {
                lane2 = Random.Range(0, 3);
            }

            // generate both obstacles
            GenerateObstacle(lane1, zObstacle);
            GenerateObstacle(lane2, zObstacle);
        }
    }

    private void GenerateObstacle(int lane, float zObstacle)
    {
        Vector3 p = new Vector3(lanes[lane], 0, zObstacle);

        Obstacle obstacleProperties;
        while (true)
        {
            // Instantiate a random obstacle from the list
            obstacleProperties = obstacles[Random.Range(0, obstacles.Count)];
            GameObject obstacleToSpawn = obstacleProperties.obstacle;

            // if obstacle is not probability dependent, just DO IT
            if (!obstacleProperties.hasProbability)
            {
                InsantiatetObjectAtPos(obstacleToSpawn, p);
                break;
            }

            // is probability dependent
            if (Random.value < (obstacleProperties.probability / 100f))
            {
                InsantiatetObjectAtPos(obstacleToSpawn, p);
                break;
            }
        }

        // if obstacle allows coins, generate it
        if (obstacleProperties.allowCoins && obstacleProperties.allowJumpCoins)
        {
            int result = Random.Range(0, 3);
            switch (result)
            {
                case 0:
                    InsantiatetObjectAtPos(coins, p);
                    break;
                case 1:
                    InsantiatetObjectAtPos(jumpCoins, p);
                    break;
            }
        } else if (obstacleProperties.allowCoins) {
            if (Random.value <= 0.5f)
                InsantiatetObjectAtPos(coins, p);
        } else if (obstacleProperties.allowJumpCoins) {
            if (Random.value <= 0.5f)
                InsantiatetObjectAtPos(jumpCoins, p);
        }


    }

    private void InsantiatetObjectAtPos(GameObject prefab, Vector3 pos)
    {
        GameObject obstacle = Instantiate(prefab);
        obstacle.transform.position = pos;
    }

    // Defines an obstacle and its properties to be edited at Unity Editor
    [Serializable]
    public class Obstacle
    {
        public GameObject obstacle;
        public bool allowCoins;
        public bool allowJumpCoins;

        public bool hasProbability;
        [Range(0, 100)]
        public int probability;
    }
}
