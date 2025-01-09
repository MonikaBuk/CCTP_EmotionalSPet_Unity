using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTest : MonoBehaviour
{
    public void IncreaseMoney(int money)
    {
        PlayerStats.AddMoney(money);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
    
}
