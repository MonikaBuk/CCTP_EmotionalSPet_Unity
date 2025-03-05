using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FishFunctions : MonoBehaviour
{
  
    public void SaveAquarium()
    {
        AquariumManagger.SaveAquarium();
    }

    public void LoadAquarium()
    {
        AquariumManagger.LoadAquarium();
    }

    public void DeleteAllPref()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Ensure changes are written to disk
        Debug.Log("All PlayerPrefs have been cleared.");
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
