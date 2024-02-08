using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatGround : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 startPos;
    public float speed = 2f;
    public GameManager gameManager;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float disChange = speed * Time.deltaTime;
        transform.Translate(Vector3.back * disChange);
        gameManager.totalScore += disChange;
        if (transform.position.z - startPos.z < -5) {
            transform.position = startPos;
        }
    }
}
