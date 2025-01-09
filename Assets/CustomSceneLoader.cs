using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class CustomSceneLoader : MonoBehaviour
{
   
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Show the loading screen
 
        // Start loading the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // Update the progress bar
        while (!operation.isDone)
        {
        

            // Activate the scene when it's ready
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        
    }
}
