using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float timeLeft = 10f;
    public float totalScore = 0f;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ejectText;
    public TextMeshProUGUI cooldownText;
    public bool gameOver = false;
    private GameObject animalPossessing;
    public GhostController ghostScript;
    
    public Camera mainCamera;
    public Camera pigCamera;
    public Camera birdCamera;
    private Camera currentCamera;
    private float nextEject = 8f;
    
    public static GameManager instance;


    void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!gameOver)
        {
            UpdateTimeLeft();
            UpdateScore();
            UpdateCamera();
            UpdateCooldown();
            UpdateEject();
        }
    }

    private void UpdateTimeLeft()
    {
        if (timeLeft > 0)
        {
            timeLeft = (timeLeft - 1 * Time.deltaTime);
            timeLeftText.text = "TimeLeft: " + "<color=red>" + timeLeft.ToString("F1") + "</color>";
        }
        else
        {
            GameOver();
        }

    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + Mathf.Round(totalScore);
    }

    private void UpdateCooldown()
    {
        float remainCooldown = Mathf.Max(0, (ghostScript.GetLastPossessionTime() + 5.0f - Time.time));
        if (remainCooldown == 0)
        {
            cooldownText.text = "Next possession available in: now";
        }
        else
        {
            cooldownText.text = "Next possession available in: " + remainCooldown.ToString("F1");
        }
    }
    
    private void UpdateEject()
    {
        if (ghostScript.animalPossessing)
        {
            animalPossessing = ghostScript.animalPossessing;
            ejectText.gameObject.SetActive(true);
            nextEject -= 1 * Time.deltaTime;
            ejectText.text = "Eject and turn back to ghost in: " + nextEject.ToString("F1");
        }
        else
        {
            ejectText.gameObject.SetActive(false);
            nextEject = 8f;
        }
    }

    private void UpdateCamera()
    {
        if (ghostScript.animalPossessing != null) {
            animalPossessing = ghostScript.animalPossessing;
            // if the animalControlling is a pig, shift to pig camera
            if (animalPossessing.name == "Pig" || animalPossessing.name == "Pig(Clone)") {
                pigCamera.gameObject.SetActive(true);
                mainCamera.gameObject.SetActive(false);
                birdCamera.gameObject.SetActive(false);
                currentCamera = pigCamera;
                Vector3 currPos = animalPossessing.transform.position;
                pigCamera.transform.position = new Vector3(currPos.x, currPos.y + 1, currPos.z - 2.5f);

            } else if(animalPossessing.name == "Bird" || animalPossessing.name == "Bird(Clone)")
            {
                birdCamera.gameObject.SetActive(true);
                mainCamera.gameObject.SetActive(false);
                pigCamera.gameObject.SetActive(false);
                currentCamera = birdCamera;
                Vector3 currPos = animalPossessing.transform.position;
                birdCamera.transform.position = new Vector3(birdCamera.transform.position.x, birdCamera.transform.position.y, animalPossessing.transform.position.z);
            } 
            //Debug.Log(animalPossessing.gameObject.name);
        }
        else //default
        {
            animalPossessing = null;
            if (currentCamera != mainCamera)
            {
                mainCamera.gameObject.SetActive(true);
                pigCamera.gameObject.SetActive(false);
                birdCamera.gameObject.SetActive(false);
                currentCamera = mainCamera;
            }
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Game over");
        gameOver = true;
        gameOverText.gameObject.SetActive(true);
    }
}
