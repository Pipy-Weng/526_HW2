using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    // Start is called before the first frame update
    private RepeatGround groundScript;
    void Start()
    {
        groundScript = GameObject.Find("Ground").gameObject.GetComponent<RepeatGround>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * groundScript.speed * Time.deltaTime);
        if (transform.position.z < -3) { Destroy(gameObject); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Animals"))
        {
            Debug.Log(collision.gameObject.tag);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}

