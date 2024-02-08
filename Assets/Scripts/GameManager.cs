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
    public bool gameOver = false; 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!gameOver)
        {
            updateTimeLeft();
            updateScore();
        }
    }

    private void updateTimeLeft()
    {
        if (timeLeft > 0)
        {
            timeLeft = (timeLeft - 1 * Time.deltaTime);
            timeLeftText.text = "TimeLeft: " + "<color=red>" + timeLeft.ToString("F1") + "</color>";
        }
        else
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
        }

    }

    private void updateScore()
    {
        scoreText.text = "Score: " + Mathf.Round(totalScore);
    }
}
