using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float possessionRange = 3f;
    private GameObject currentTarget = null;
    private List<GameObject> nearbyAnimals = new List<GameObject>();
    private float possessionCooldown = 5f;
    private float lastPossessionTime = -5f;

    void Update()
    {
        // always find and highlight the nearest animal
        FindAndHighlightAnimals();
        
        HandlePossession();
    }

    void HandlePossession()
    {
        if (Time.time >= lastPossessionTime + possessionCooldown && Input.GetKeyDown(KeyCode.Return) && currentTarget != null)
        {
            PossessAnimal();
        }

        if (currentTarget != null)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeTarget(1); // further target
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeTarget(-1); // closer target
            }
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
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestAnimal = hit.gameObject;
                }
            }
        }
        
        if (nearestAnimal != null && nearestAnimal != currentTarget)
        {
            if (currentTarget != null)
            {
                // Unhighlight previous target if any
                SetHighlight(currentTarget, false);
            }
            // Highlight the new nearest animal
            currentTarget = nearestAnimal;
            SetHighlight(currentTarget, true);
        }
        
        // all animals are out of range
        else if (nearestAnimal == null && currentTarget != null)
        {
            SetHighlight(currentTarget, false);
            currentTarget = null;
        }
    }
    
    void ChangeTarget(int distance)
    {
        if (nearbyAnimals.Count == 0)
        {
            // exit if no animals
            return;
        }
        
        int currentIndex = nearbyAnimals.IndexOf(currentTarget);
        
        // ensure currentIndex is valid
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }
        
        // remove highlight from current target
        SetHighlight(currentTarget, false);
        
        int targetIndex = (currentIndex + distance) % nearbyAnimals.Count;
        if (targetIndex < 0) targetIndex += nearbyAnimals.Count;
        
        // change and highlight the new target
        if (targetIndex >= 0 && targetIndex < nearbyAnimals.Count)
        {
            currentTarget = nearbyAnimals[targetIndex];
            SetHighlight(currentTarget, true);
        }
        else
        {
            Debug.LogError("Target index is out of bounds: " + targetIndex);
        }
    }
    
    
    void SetHighlight(GameObject target, bool highlight)
    {
        // change the target's outline color to show highlight
        if (highlight)
        {
            target.GetComponent<MeshRenderer>().material.color =Color.green;
        }
        else
        {
            // load the material by name
            if (target.name is "Pig(Clone)" or "Pig")
            {
                Material originalMaterial = Resources.Load<Material>("Material/pig");
                if (originalMaterial != null)
                {
                    target.GetComponent<MeshRenderer>().material = originalMaterial;
                }
            }
            if (target.name is "Bird(Clone)" or "Bird")
            {
                Material originalMaterial = Resources.Load<Material>("Material/Bird");
                if (originalMaterial != null)
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
        // TODO: the transformed position is not the same as the target ;(
        transform.position = currentTarget.transform.position;
        
        // enabling animal control
        var animalControl = currentTarget.GetComponent<PlayerController>();
        if (animalControl != null)
        {
            animalControl.isPossessed = true;
            
            // let ghost follow the animal
            this.transform.SetParent(currentTarget.transform);
        }
        
        // unhighlight and clear target
        SetHighlight(currentTarget, false);
        currentTarget = null;
    }
}
