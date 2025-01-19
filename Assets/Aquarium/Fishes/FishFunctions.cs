using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FishFunctions : MonoBehaviour
{
  public void AddNewFishTest(string fishName)
    {
        AquariumManagger.AddNewFish(fishName);
    }

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
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
