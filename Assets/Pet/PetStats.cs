using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PetStats : MonoBehaviour
{
    public static PetStats Instance { get; private set; }
    private int maxStat = 100;

    public float statDecreaseRate = 0.1f;

    private int fullness;
    private int joyness;
    private int cleaness;

    private float hunger;
    private float dirtyness;
    private float boredom;

    public static bool wasFed = false;
    public static bool wasCleaned = false;
    public static bool wasPlayed = false;



    private DateTime lastFedTime;
    private DateTime lastCleanTime;
    private DateTime lastPlayTime;

    private AnimationManager myAnimManager;

    [SerializeField] StatBar foodBar;
    [SerializeField] StatBar cleanBar;
    [SerializeField] StatBar playBar;

    [SerializeField] private Material material;

    void Start()
    {
        myAnimManager = GetComponent<AnimationManager>();
        LoadData();
        InvokeRepeating("Calculate", 0f, 1f);
    }

    private void Update()
    {
        if (wasFed)
        {
            FeedPet();
            wasFed = false;
            StartCoroutine(ResetAnimationAfterDelay(2.0f));
        }
        if (wasCleaned)
        {
            CleanPet();
            wasCleaned = false;
            StartCoroutine(ResetAnimationAfterDelay(2.0f));

        }
        if (wasPlayed)
        {
            PlayWithnPet();
            wasPlayed = false;
            StartCoroutine(ResetAnimationAfterDelay(2.0f));
        }
    }


    void CalculateHunger(DateTime currentTime)
    {
        TimeSpan timeElapsed = currentTime - lastFedTime;
        hunger = (float)timeElapsed.TotalSeconds * statDecreaseRate;
        hunger = Mathf.Clamp(hunger, 0, maxStat); 
        fullness = maxStat - (int)hunger;
        foodBar.UpdateStatBar(fullness);
    }

    void CalculateDirtyness(DateTime currentTime)
    {
        TimeSpan timeElapsed = currentTime - lastCleanTime;
        dirtyness = (float)timeElapsed.TotalSeconds * statDecreaseRate;
        dirtyness = Mathf.Clamp(dirtyness, 0, maxStat);
        cleaness = maxStat - (int)dirtyness;
        cleanBar.UpdateStatBar(cleaness);

        float clean = dirtyness / 100.0f;
        Debug.Log(clean);
      
       material.SetFloat("_Dirtiness", clean);
    }

    void CalculateJoyness(DateTime currentTime)
    {
        TimeSpan timeElapsed = currentTime - lastPlayTime;
        boredom = (float)timeElapsed.TotalSeconds * statDecreaseRate;
        boredom = Mathf.Clamp(boredom, 0, maxStat);
        joyness = maxStat - (int)boredom;
        playBar.UpdateStatBar(joyness);
    }

    void Calculate()
    {
        DateTime currentTime = DateTime.Now;
        CalculateHunger(currentTime);
        CalculateDirtyness(currentTime);
        CalculateJoyness(currentTime);
    }

    public void FeedPet()
    {
        hunger = 0;
        lastFedTime = DateTime.Now;
        myAnimManager.SetAnimationId(5);
        PlayerPrefs.SetString("LastFedTime", lastFedTime.ToString());
    }
    public void CleanPet()
    {
        dirtyness = 0;
        lastCleanTime = DateTime.Now;
        myAnimManager.SetAnimationId(7);
        material.SetFloat("_Dirtiness", 0);
        PlayerPrefs.SetString("LastCleanTime", lastCleanTime.ToString());
    }
    public void PlayWithnPet()
    {
        boredom = 0;
        lastPlayTime = DateTime.Now;
        myAnimManager.SetAnimationId(3);
        PlayerPrefs.SetString("LastPlayTime", lastPlayTime.ToString());

    }
    public void SaveData()
    {
        PlayerPrefs.SetString("LastFedTime", lastFedTime.ToString());
        PlayerPrefs.SetString("LastCleanTime", lastCleanTime.ToString());
        PlayerPrefs.SetString("LastPlayTime", lastPlayTime.ToString());
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("LastFedTime"))
        {
            lastFedTime = DateTime.Parse(PlayerPrefs.GetString("LastFedTime"));
        }
        else
        {
            lastFedTime = DateTime.Now;
        }

        if (PlayerPrefs.HasKey("LastCleanTime"))
        {
            lastCleanTime = DateTime.Parse(PlayerPrefs.GetString("LastCleanTime"));
        }
        else
        {
            lastCleanTime = DateTime.Now;
        }
        if (PlayerPrefs.HasKey("LastPlayTime"))
        {
            lastPlayTime = DateTime.Parse(PlayerPrefs.GetString("LastPlayTime"));
        }
        else
        {
            lastPlayTime = DateTime.Now;
        }
    }

    private IEnumerator ResetAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myAnimManager.SetAnimationId(0); 
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

   


}
