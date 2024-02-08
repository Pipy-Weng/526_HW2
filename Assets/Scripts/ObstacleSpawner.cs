using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> obstacles;
    private float startDelay = 5;
    private float repeatRate = 10;
    void Start()
    {
        InvokeRepeating("SpawnObstacles", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacles()
    {
        float range = Random.Range(0,1);
        GameObject obstacle;
        if (range > 0.75)
        {
            obstacle = obstacles[0]; //high obstacle
        } else
        {
            obstacle = obstacles[1]; //low obstacle
        }
            Vector3 spawnPos = new Vector3(0, -0.5f, 10);
            Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
    }
}
