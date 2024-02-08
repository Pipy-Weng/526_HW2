using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> animals;
    private float startDelay = 1;
    private float repeatRate = 3;
    void Start()
    {
        InvokeRepeating("SpawnAnimals", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update() {

    }

    void SpawnAnimals()
    {
        GameObject animal = animals[Random.Range(0,2)];

        if(animal.name == "Pig") {
            Vector3 spawnPos = new Vector3(Random.Range(-1.8f,1.8f), 0, -10);
            Instantiate(animal, spawnPos, animal.transform.rotation);
        }

        if(animal.name == "Bird")
        {
            Vector3 spawnPos = new Vector3(Random.Range(-1.8f, 1.8f), 2.5f, -10);
            Instantiate(animal, spawnPos, animal.transform.rotation);
        }
    }
}
