using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LevelLoadManager : MonoBehaviour
{

    public Slider loadingProgress;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneAsync("GameLevel"));
    }


    IEnumerator LoadSceneAsync(string levelName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            //float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);
            yield return null;
        }
    }
}

