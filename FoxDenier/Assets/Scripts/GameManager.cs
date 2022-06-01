using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Slider timeBar;
    public TextMeshProUGUI chickensRemaining;
    public TextMeshProUGUI roundCounter;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI congratulationsText;
    public Button restartButton;
    public float roundTime = 15.0f;
    private float timer;
    public Object[] chickenCounter;
    public List<GameObject> rounds;
    private Object[] barriers;
    public int currentRound;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        timer = roundTime;
        currentRound = 0;
        rounds[currentRound].SetActive(true);
        roundCounter.text = "ROUND: " + (currentRound + 1);

    }

    // Update is called once per frame
    void Update()
    {
        chickenCounter = FindObjectsOfType<ChickenAnimal>(false);
        chickensRemaining.text = "CHICKENS REMAINING: " + chickenCounter.Length;

        if (timer > 0 && !gameOver)
        {
            timer -= Time.deltaTime;
            timeBar.value = Mathf.InverseLerp(0, roundTime, timer);
            if (timer <= 0)
            {
                NextRound();
                timer = roundTime;
            }
        }

        if (chickenCounter.Length <= 0)
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

        }
    }

    private void NextRound()
    {
        rounds[currentRound].SetActive(false);
        roundCounter.text = "ROUND: " + (currentRound + 1);
        barriers = FindObjectsOfType<BarrierHandler>(false);
        foreach (BarrierHandler barrier in barriers)
        {
            Destroy(barrier.gameObject);
        }
        currentRound += 1;
        if (currentRound <= rounds.Count)
        {
            rounds[currentRound].SetActive(true);
        } else
        {
            congratulationsText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
