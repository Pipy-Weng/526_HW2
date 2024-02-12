using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    // Start is called before the first frame update
    private float animalSpeed;
    private string animalType;
    public float jumpForce = 5.0f;
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
        CheckPossessed();
        transform.Translate(Vector3.forward * (animalSpeed * Time.deltaTime));
        if(transform.position.z > 10) { Destroy(gameObject); }
        
        if (isPossessed && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (isPossessed && Input.GetKeyDown(KeyCode.E))
        {
            isPossessed = false;
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

    void CheckPossessed()
    {
        if (isPossessed) {
            animalSpeed = 0;
        }
    }
    
    void Jump()
    {
        Debug.Log("jump");
        if (_rigid != null && isPossessed)
        {
            // birds can always "flap" to jump, can only jump once
            if (animalType is "Bird(Clone)" or "Bird") 
            {
                StartCoroutine(BirdJumpRoutine(6.0f, 2.3f, 0.7f));
                isOnGround = false;
            }
            
            // pigs only jump if on the ground and can only jump once
            else if (animalType is "Pig(Clone)" or "Pig") 
            {
                _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }
        }
    }
    
    
    IEnumerator BirdJumpRoutine(float targetHeight, float returnHeight, float timeToReachTarget)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetHeight, startPos.z);

        // jump to the target height
        while (elapsedTime < timeToReachTarget)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / timeToReachTarget));
            elapsedTime += Time.smoothDeltaTime;
            yield return null;
        }

        // Snap to the target height in case of minor error in Lerp calculation
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);

        // back to original height
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, returnHeight, startPos.z);
        elapsedTime = 0f;

        // use the same duration for going down as it took to go up
        while (elapsedTime < timeToReachTarget)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / timeToReachTarget));
            elapsedTime += Time.smoothDeltaTime;
            yield return null;
        }

        // ensure the bird is exactly at the return height
        transform.position = new Vector3(transform.position.x, returnHeight, transform.position.z);
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
