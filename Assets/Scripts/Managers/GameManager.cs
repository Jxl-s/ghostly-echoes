using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string GAME_VERSION = "0.0.1";
    public static GameManager Instance;

    public float BatteryPercentage = 100;
    public float StaminaPercentage = 100;
    public float HealthPercentage = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
