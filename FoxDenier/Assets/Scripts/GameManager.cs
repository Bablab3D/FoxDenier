using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float roundTime = 15.0f;
    public Slider timeBar;
    public TextMeshProUGUI chickensRemaining;
    public TextMeshProUGUI roundCounter;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI congratulationsText;
    public Button restartButton;
    private float timer;
    public Object[] chickenCounter;
    public List<GameObject> rounds;
    private Object[] barriers;
    public int currentRound;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        // start first round
        timer = roundTime;
        currentRound = 0;
        rounds[currentRound].SetActive(true);
        roundCounter.text = "ROUND: " + (currentRound + 1);

    }

    // Update is called once per frame
    void Update()
    {
        // look for all chickens
        chickenCounter = FindObjectsOfType<ChickenAnimal>(false);
        chickensRemaining.text = "CHICKENS REMAINING: " + chickenCounter.Length;
        
        if (timer > 0 && !gameOver)
        {
            timer -= Time.deltaTime;

            // make the timebar slider go down to show how much time is remaining
            timeBar.value = Mathf.InverseLerp(0, roundTime, timer);
            if (timer <= 0)
            {
                // start next round and reset timer
                NextRound();
                timer = roundTime;
            }
        }

        // if there are no more chickens, game over
        if (chickenCounter.Length <= 0 && currentRound <= rounds.Count - 1)
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

        }
    }

    // ABSTRACTION
    private void NextRound()
    {
        // each 'round' is an empty parent with some foxes and chickens in it,
        // so the next round is just switching off the current round empty and switching on the next one.
        rounds[currentRound].SetActive(false);
        roundCounter.text = "ROUND: " + (currentRound + 1);

        // find all the barriers and destroy them.
        barriers = FindObjectsOfType<BarrierHandler>(false);
        foreach (BarrierHandler barrier in barriers)
        {
            Destroy(barrier.gameObject);
        }
        currentRound += 1;
        if (currentRound <= rounds.Count - 1)
        {
            rounds[currentRound].SetActive(true);
        } else
        {
            congratulationsText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            gameOver = true;
        }
    }

    public void RestartGameCoroutine()
    {
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        // I was having problems with the lighting when loading scenes but I think this fixes it.
        // Thanks to radiatoryang on the unity forums (and thanks to radiatoryang for being an awesome games person in general)
        SceneManager.LoadScene(0);
        yield return 0; // wait a frame, so it can finish loading
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

        // oh wait this didn't fix it. I fixed it by autogenerating lighting because it was unchecked but all the forums said to uncheck it? idk how this works.
    }
}
