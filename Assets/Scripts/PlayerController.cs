using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float animalSpeed;
    private string animalType;
    private float jumpForce = 7.5f;
    public bool isPossessed;
    private bool isOnGround = true;
    private Rigidbody _rigid;
    private GameManager gameManager;
    public float possessionTimer = 0f;
    private GameObject ghost;
    
    private void Awake()
    {
        animalType = gameObject.name;
        _rigid = gameObject.GetComponent<Rigidbody>();
        if (animalType == "Pig(Clone)")
        {
            animalSpeed = 3f;
        }
        if (animalType == "Bird(Clone)")
        {
            animalSpeed = 2f;
        }
        
    }
    void Start()
    {
        ghost = GameObject.Find("Ghost");
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        checkPossessed();
        transform.Translate(Vector3.forward * animalSpeed * Time.deltaTime);
        if(transform.position.z > 10) { Destroy(gameObject); }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (isPossessed && Input.GetKeyDown(KeyCode.E))
        {
            isPossessed = false;
            if (_rigid.name is "Bird" or "Bird(Clone)")
            {
                _rigid.useGravity = false;
            }
        }
        
        // Birds always fly in the sky
        if (gameObject.name is "Bird(Clone)" or "Bird")
        {
            if (transform.position.y < 2.3f)
            {
                var vector3 = transform.position;
                vector3.y = 2.3f;
                transform.position = vector3;
            }
        }


        CheckEject();

    }

    void checkPossessed()
    {
        if (isPossessed) {
            _rigid.useGravity = true;
            animalSpeed = 0;
        }
    }
    
    void Jump()
    {

        if (_rigid != null && isPossessed)
        {
            //TODO: if the animal is bird? bird does not have Gravity so it nto comeback
            if ((gameObject.name == "Pig(Clone)" || gameObject.name == "Pig") && isOnGround) {
                _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (gameObject.name == "Bird(Clone)" || gameObject.name == "Bird")
            {
                _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void CheckEject()
    {
        if (isPossessed)
        {
            possessionTimer += Time.deltaTime;
            if (possessionTimer >= 8.0f)
            {
                ghost.GetComponent<GhostController>().LeaveAnimal();
            }
        }
        else
        {
            possessionTimer = 0f;
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (isPossessed && collision.gameObject.CompareTag("Animals"))
        {
            Debug.Log("Hit anther animal, game over");
            gameManager.GetComponent<GameManager>().GameOver();
        }
        else
        {
            isOnGround = true;
        }
    }
}
