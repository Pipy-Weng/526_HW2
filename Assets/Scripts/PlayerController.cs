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
    
    private void Awake()
    {
        animalType = gameObject.name;
        _rigid = gameObject.GetComponent<Rigidbody>();
        if (animalType == "Pig(Clone)")
        {
            animalSpeed = 3;
        }
        if (animalType == "Bird(Clone)")
        {
            animalSpeed = 2;
        }
    }
    void Start()
    {

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
            if (_rigid.name == "Bird" || _rigid.name == "Bird(Clone)")
            {
                _rigid.useGravity = false;
            }
        }


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


    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
    }
}
