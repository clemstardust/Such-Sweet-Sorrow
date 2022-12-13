using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadAfterTime : MonoBehaviour
{
    private float delayBeforeLoading;
    private string sceneNameToLoad;

    private float timeElapsed;

    private bool startCountdown;
    private bool startSecondCountdown;

    public GameObject deathScreen;

    private void Start()
    {
        deathScreen.SetActive(false);
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
            
        if (timeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    public void LoadSceneAfterDelay(float delay, string sceneToLoad)
    {
        sceneNameToLoad = sceneToLoad;
        delayBeforeLoading = 1;
        startCountdown = true;
    }
}