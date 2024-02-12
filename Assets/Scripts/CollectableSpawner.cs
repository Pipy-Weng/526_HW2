using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ghost;
    public List<GameObject> collectables;
    private float startDelay = 5;
    private float repeatRate = 10;
    void Start()
    {
        InvokeRepeating("SpawnCollectables", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCollectables()
    {
        int range = Random.Range(0, 2);
        GameObject collectable;
        Vector3 spawnPos;
        int randomNum = Random.Range(1,4);
        for (int i = 0; i < randomNum; i++ ) { //randomly spawn 1-3 collectables
            if (range == 0) {
                collectable = collectables[0]; // 0 is ground collectable
                // avoid overlapping obstacles
                spawnPos = new Vector3(ghost.transform.position.x, 0, 12 + i * 2);
            } else
            {
                collectable = collectables[1]; // 1 is sky collectable
                spawnPos = new Vector3(ghost.transform.position.x, 4 + i*0.5f, 10 + i * 2);
            }
            Instantiate(collectable, spawnPos, collectable.transform.rotation);
        }
    }
}
