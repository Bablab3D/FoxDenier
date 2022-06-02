using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI title;
    public TextMeshProUGUI credit;

    public List<Color> colors;
    [SerializeField] private float colorDuration;
    private float t1;
    private float t2;
    private int c1;
    private int c2;

    // Start is called before the first frame update
    void Start()
    {
        colors.Add(Color.red);
        colors.Add(Color.magenta);
        colors.Add(Color.blue);
        colors.Add(Color.cyan);
        colors.Add(Color.green);
        colors.Add(Color.yellow);

        t1 = colorDuration;
        t2 = colorDuration * 0.9f;
        c1 = 0;
        c2 = 1;

    }

    // Update is called once per frame
    void Update()
    {
        ColorHandler();
    }

    // ABSTRACTION
    private void ColorHandler()
    {
        if (t1 > 0)
        {
            t1 -= Time.deltaTime;
            t2 -= Time.deltaTime;
            title.color = Color.Lerp(colors[c1], colors[c2], 1 - Mathf.InverseLerp(0, colorDuration, t1));
            credit.color = Color.Lerp(colors[c1], colors[c2], 1 - Mathf.InverseLerp(0, colorDuration, t2));

            if (t1 <= 0)
            {
                if (c1 < colors.Count - 1)
                {
                    c1 += 1;
                }
                else
                {
                    c1 = 0;
                }

                if (c2 < colors.Count - 1)
                {
                    c2 += 1;
                }
                else
                {
                    c2 = 0;
                }

                t1 = colorDuration;
                t2 = colorDuration * 0.9f;
            }
        }
    }

    public void StartGameCoroutine()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        // I was having problems with the lighting when loading scenes but I think this fixes it.
        // Thanks to radiatoryang on the unity forums (and thanks to radiatoryang for being an awesome games person in general)
        SceneManager.LoadScene(1);
        yield return 0; // wait a frame, so it can finish loading
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));

        // oh wait this didn't fix it. I fixed it by autogenerating lighting because it was unchecked but all the forums said to uncheck it? idk how this works.
    }
}
