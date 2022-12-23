using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadAfterTime : MonoBehaviour
{
    private float delayBeforeLoading;
    private string sceneNameToLoad;

    private float timeElapsed;

    private bool startCountdown;
    private bool startSecondCountdown;
    private bool isLoading;

    public GameObject deathScreen;

    private void Start()
    {
        deathScreen.SetActive(false);
        isLoading = false;
    }

    void Update()
    {
        if (startCountdown)
        {
            deathScreen.SetActive(true);
            deathScreen.GetComponent<CanvasGroup>().alpha += 0.2f * Time.deltaTime;
        }
        if (deathScreen.GetComponent<CanvasGroup>().alpha >= 1)
        {
            startSecondCountdown = true;
        }
        if (startSecondCountdown)
        {
            timeElapsed += Time.deltaTime;
        }
            
        if (timeElapsed > delayBeforeLoading && !isLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);
            isLoading = true;
            startCountdown = false;
            timeElapsed = 0;
            //deathScreen.GetComponent<CanvasGroup>().alpha = 0;
            startSecondCountdown = false;
        }
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startCountdown = false;
        timeElapsed = 0;
        deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        startSecondCountdown = false;
        print("ran on scene loaded from LoadAfterTime");
    }

    public void ResetLoading()
    {
        startCountdown = false;
        timeElapsed = 0;
        deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        startSecondCountdown = false;
    }

    public void LoadSceneAfterDelay(float delay, string sceneToLoad)
    {
        sceneNameToLoad = sceneToLoad;
        delayBeforeLoading = 1;
        startCountdown = true;
    }
}