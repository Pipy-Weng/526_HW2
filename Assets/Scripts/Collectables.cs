using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    private RepeatGround groundScript;
    void Start()
    {
        groundScript = GameObject.Find("Ground").gameObject.GetComponent<RepeatGround>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * groundScript.speed * Time.deltaTime);
        if (transform.position.z < -3) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("SkyCollectable") && other.transform.childCount != 0)
        {
            gameManager.GetComponent<GameManager>().timeLeft += 3; // skycollectable increase the timeleft
        }
        if (gameObject.CompareTag("GroundCollectable") && other.transform.childCount != 0)
        {
            gameManager.GetComponent<GameManager>().totalScore += 5; //ground collectable increase the total score
        }
        Destroy(gameObject);
    }
}
