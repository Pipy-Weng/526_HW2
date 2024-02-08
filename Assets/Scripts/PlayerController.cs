using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float animalSpeed;
    private string animalType;
    private float jumpForce = 5f;
    public bool isPossessed = false;

    private Rigidbody _rigid;
    
    private void Awake()
    {
        animalType = gameObject.name;
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
        _rigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * animalSpeed * Time.deltaTime);
        if(transform.position.z > 10) { Destroy(gameObject); }
        
        if (isPossessed && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    
    void Jump()
    {
        if (_rigid != null)
        {
            //TODO: if the animal is bird? bird does not have Gravity so it nto comeback
            _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }
    }

}
