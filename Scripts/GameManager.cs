
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hazardPrefab;
    [SerializeField]
    private int maxHazardsToSpawn = 3;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private Image backgroundMenu;
    [SerializeField]
    private GameObject mainVCam;
    [SerializeField]
    private GameObject zoomVCam;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject player;

    private int score = 0;
    private int highScore;
    private float timer;
    private Coroutine hazardsCoroutine;
    private  bool gameOver;
    
    
    private static GameManager instance;
    private const string HighScorePreferenceKey = "HighScore";

    public static GameManager Instance => instance;
    public int HighScore => highScore;

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);
    }

    private void OnEnable() 
    {


        player.SetActive(true);   
        zoomVCam.SetActive(false);
        mainVCam.SetActive(true); 
        gameOver = false;
        scoreText.text = "0";
        score = 0;
        timer = 0;
        hazardsCoroutine = StartCoroutine(SpawnHazards());
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                unPause();

            }
            if (Time.timeScale == 1)
            {
                Pause();
            }
        }

        if (gameOver)
            return;

        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            score++;
            scoreText.text = score.ToString();
            timer = 0;
        }    
    }

    private void unPause()
    {
        LeanTween.value(0, 1, 0.75f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
        // StartCoroutine(ScaleTime(0, 1, 0.5f));
        backgroundMenu.gameObject.SetActive(false);
    }

    private void Pause()
    {
        LeanTween.value(1, 0, 0.75f).setOnUpdate(SetTimeScale).setIgnoreTimeScale(true);
        // StartCoroutine(ScaleTime(0, 1, 0.5f));
        backgroundMenu.gameObject.SetActive(false);
    }

    private void SetTimeScale(float value)
    {
       Time.timeScale=value;
        Time.fixedDeltaTime = 0.02f * value;
    }
/*
    IEnumerator ScaleTime(float start, float end, float duration)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < duration)
        {
            Time.timeScale = Mathf.Lerp(start,end,timer/duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale=end;
        Time.fixedDeltaTime = 0.02f * end;

    }
*/
    private IEnumerator SpawnHazards()
    {
        var hazardToSpawn = Random.Range(1,maxHazardsToSpawn);

        for (int i = 0; i < hazardToSpawn; i++)
        {
            var x = Random.Range(-7, 7);
            var drag = Random.Range(0f, 2f);
            var hazard =  Instantiate(hazardPrefab, new Vector3(x, 11, 0), Quaternion.identity);
            hazard.GetComponent<Rigidbody>().drag = drag;
        }

        yield return new WaitForSeconds(1f);

        yield return SpawnHazards();

    }

    public void GameOver()
    {
        StopCoroutine(hazardsCoroutine);
        gameOver = true;

        if(Time.timeScale < 1)
        {
           unPause();
        }

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);
            Debug.Log(highScore);
        }

        mainVCam.SetActive(false);
        zoomVCam.SetActive(true);
        gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
       
    }
    public void Enable()
    {
        gameObject.SetActive(true);
        
    }
}
