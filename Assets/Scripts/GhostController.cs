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
        HandlePossession();
    }

    void HandlePossession()
    {
        if (Time.time >= lastPossessionTime + possessionCooldown && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("HandlePossession");
            if (currentTarget == null)
            {
                FindAndHighlightAnimals();
            }
            else
            {
                PossessAnimal();
            }
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
        Debug.Log("FindAndHighlightAnimals");
        Collider[] hits = Physics.OverlapSphere(transform.position, possessionRange);
        nearbyAnimals.Clear();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Animals"))
            {
                nearbyAnimals.Add(hit.gameObject);
            
                // only highlight the first animal
                if (nearbyAnimals.Count == 1)
                {
                    currentTarget = hit.gameObject;
                    SetHighlight(currentTarget, true);
                }
            }
        }
        
        // no animals are found or all animals are out of range
        if (nearbyAnimals.Count == 0)
        {
            currentTarget = null;
        }
    }
    
    void ChangeTarget(int distance)
    {
        int currentIndex = nearbyAnimals.IndexOf(currentTarget);
        
        // remove highlight from current target
        SetHighlight(currentTarget, false);
        
        int targetIndex = currentIndex + distance;
        if (targetIndex >= nearbyAnimals.Count) targetIndex = 0;
        else if (targetIndex < 0) targetIndex = nearbyAnimals.Count - 1;
        
        // change and highlight the new target
        currentTarget = nearbyAnimals[targetIndex];
        SetHighlight(currentTarget, true); 
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
        Debug.Log("PossessAnimal");
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
