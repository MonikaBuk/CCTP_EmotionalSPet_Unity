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

    public static bool wasFed;


    private DateTime lastFedTime;
    private DateTime lastCleanTime;
    private DateTime lastPlayTime;

    private AnimationManager myAnimManager;

    [SerializeField] StatBar foodBar;
    [SerializeField] StatBar cleanBar;
    [SerializeField] StatBar playBar;


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
        StartCoroutine(RevertToBasicAnimation());
        PlayerPrefs.SetString("LastFedTime", lastFedTime.ToString());
    }
    public void CleanPet()
    {
        dirtyness = 0;
        lastCleanTime = DateTime.Now;
        myAnimManager.SetAnimationId(7);
        StartCoroutine(RevertToBasicAnimation());
    }
    public void PlayWithnPet()
    {
        boredom = 0;
        lastPlayTime = DateTime.Now;
        myAnimManager.SetAnimationId(3);
        StartCoroutine(RevertToBasicAnimation());
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

    void OnApplicationQuit()
    {
        SaveData();
    }

    private IEnumerator RevertToBasicAnimation()
    {
  
        yield return new WaitForSeconds(5f);

        if (IsPetHappy())
        {
            myAnimManager.SetAnimationId(1); 
        }
        else
        {
            myAnimManager.SetAnimationId(0);
        }
    }

    private bool IsPetHappy()
    {
        return hunger < 50; 
    }
}
