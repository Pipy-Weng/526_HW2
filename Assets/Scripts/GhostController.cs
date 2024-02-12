using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float possessionRange = 3f;
    private GameObject currentTarget = null;
    private List<GameObject> nearbyAnimals = new List<GameObject>();
    public float possessionCooldown = 5f;
    private float lastPossessionTime = -5f;
    public GameObject animalPossessing;
    private Vector3 startPos = new Vector3(0, 1.84f, 0);
    public GameManager gameManager;
    public bool isPossess = false;

    void Update()
    {
        // always find and highlight the nearest animal
        FindAndHighlightAnimals();
        HandlePossession();

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("E pressed");
            LeaveAnimal();
        }
    }

    void HandlePossession()
    {
        if (Time.time >= lastPossessionTime + possessionCooldown && Input.GetKeyDown(KeyCode.Return) && currentTarget != null && !isPossess)
        {
            PossessAnimal();
        }
    }


    void FindAndHighlightAnimals()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, possessionRange);
        GameObject nearestAnimal = null;
        float nearestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Animals"))
            {
                // update nearest
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < nearestDistance && distance != 0)
                {
                    nearestDistance = distance;
                    nearestAnimal = hit.gameObject;
                }
            }
        }

        if (nearestAnimal && nearestAnimal != currentTarget)
        {
            if (currentTarget)
            {
                // Unhighlight previous target if any
                SetHighlight(currentTarget, false);
            }
            // Highlight the new nearest animal
            currentTarget = nearestAnimal;
            SetHighlight(currentTarget, true);
        }

        // all animals are out of range
        else if (nearestAnimal == null && currentTarget)
        {
            SetHighlight(currentTarget, false);
            currentTarget = null;
        }
    }
    
    void SetHighlight(GameObject target, bool highlight)
    {
        // change the target's outline color to show highlight
        if (highlight)
        {
            target.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            // load the material by name
            if (target.name is "Pig(Clone)" or "Pig")
            {
                Material originalMaterial = Resources.Load<Material>("Material/pig");
                if (originalMaterial)
                {
                    target.GetComponent<MeshRenderer>().material = originalMaterial;
                }
            }
            if (target.name is "Bird(Clone)" or "Bird")
            {
                Material originalMaterial = Resources.Load<Material>("Material/Bird");
                if (originalMaterial)
                {
                    target.GetComponent<MeshRenderer>().material = originalMaterial;
                }
            }

        }
    }


    void PossessAnimal()
    {
        // initiate cooldown
        lastPossessionTime = Time.time;
        
        // moving to the target position
        transform.position = currentTarget.transform.position;
        
        // enabling animal control
        animalPossessing = currentTarget;
        var animalControl = currentTarget.GetComponent<AnimalController>();
        
        if (animalControl)
        {
            animalControl.isPossessed = true;
            isPossess = true;
        
            // let ghost follow the animal
            this.transform.SetParent(currentTarget.transform);
            gameManager.timeLeft += 10;
        }
        
        // unhighlight and clear target
        SetHighlight(currentTarget, false);
        currentTarget = null;
    }

    public void LeaveAnimal()
    {
        //Leave the embodied animal
        if (animalPossessing) {
            Transform child = animalPossessing.transform.GetChild(0);
            child.position = startPos;
            Transform tmp = child.parent;
            child.SetParent(null);
            Destroy(tmp.gameObject);
            animalPossessing = null;
        }

        isPossess = false;
    }

    public float GetLastPossessionTime()
    {
        return lastPossessionTime;
    }
}