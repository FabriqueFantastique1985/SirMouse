using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviourSingleton<Loader>
{
    public IEnumerator LoadScene(string sceneName, bool withFade, float fadeTime = 3.0f, float waitTime = 0.0f)
    {
        AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;


        if (withFade)
        {
         //   UIPanel.Instance.FadeToBlack(true, fadeTime);
            yield return new WaitForSeconds(fadeTime);
        }   
       
        yield return new WaitForSeconds(waitTime);

        if (loadOperation.progress >= 0.9f)
        {
            yield return null;
            loadOperation.allowSceneActivation = true;
        }

        if (withFade)
        {
        //    UIPanel.Instance.FadeToBlack(false, fadeTime);
        }
    }

    // To change or to delete
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}